using Microsoft.Data.SqlClient;
using Dapper;
using Shopping.lib.Enums;
using Shopping.lib.Helpers;
using ShoppingBackstage.Areas.Account.Services.Interface;
using ShoppingBackstage.Areas.Account.ViewModels;
using ShoppingBackstage.Enums;

namespace ShoppingBackstage.Areas.Account.Services.Service;

public class AccountManagementService : IAccountManagementService
{
    private readonly SqlConnection _con;

    // Constructor
    public AccountManagementService(SqlConnection connection)
    {
        _con = connection;
    }

    /// <summary>
    /// 取得 列表
    /// </summary>
    /// <returns></returns>
    public List<AdminAccountViewModels> GetList()
    {
        var sql = @"SELECT * FROM a0001_adminAccount WHERE delete_ = 0";

        var result = _con.Query<AdminAccountViewModels>(sql).ToList();
        return result;
    }

    /// <summary>
    /// 取得 單一資料
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public AdminAccountViewModels Get( Guid id )
    {
        var sql = @"SELECT * FROM a0001_adminAccount WHERE delete_ = 0 AND id_ = @id";

        var result = _con.QueryFirstOrDefault<AdminAccountViewModels>(sql, new { id });

        return result ?? new AdminAccountViewModels();
    }

    /// <summary>
    /// 檢查 使用者帳號是否重複
    /// </summary>
    /// <param name="id"></param>
    /// <param name="account"></param>
    /// <returns></returns>
    public bool IsAccountExists(Guid id, string account)
    {
        var sql = @"SELECT 
                    CASE 
                        WHEN EXISTS (
                            SELECT 1 
                            FROM a0001_adminAccount
                            WHERE account_ = @account
                                AND id_ != @id
			                    AND delete_ = 0
                        ) THEN 1 
                        ELSE 0
                    END AS IsExists;";
    
        return _con.QueryFirstOrDefault<bool>(sql, new 
        { 
            account,
            id
        });
    }

    /// <summary>
    /// 新增、更新
    /// </summary>
    /// <param name="model"></param>
    public void Save(AdminAccountViewModels model)
    {
        // 如果目標表中存在具有相同 id_ 的記錄，則更新該記錄；如果不存在，則插入新記錄
        var sql = @"MERGE a0001_adminAccount AS target
                    USING (SELECT 
                            @id_ AS id_, 
                            @account_ AS account_, 
                            @password_ AS password_, 
                            @username_ AS username_, 
                            @phone_ AS phone_, 
                            @email_ AS email_, 
                            @delete_ AS delete_, 
                            @create_time_ AS create_time_, 
                            @modify_time_ AS modify_time_, 
                            @modify_id_ AS modify_id_) AS source
                    ON (target.id_ = source.id_)
                    WHEN MATCHED THEN
                        UPDATE SET 
                            account_ = source.account_,
                            password_ = source.password_,
                            username_ = source.username_,
                            phone_ = source.phone_,
                            email_ = source.email_,
                            modify_time_ = source.modify_time_,
                            modify_id_ = source.modify_id_
                    WHEN NOT MATCHED THEN
                        INSERT (id_, account_, password_, username_, phone_, email_, delete_, create_time_, modify_time_, modify_id_)
                        VALUES (source.id_, source.account_, source.password_, source.username_, source.phone_, source.email_, source.delete_, source.create_time_, source.modify_time_, source.modify_id_);
                    ";
        
        if (model.actionType_ == ActionType.Add.ToInt()) // 如果是新增的記錄，設置 id_ 和 create_time_
        {
            model.id_ = Guid.NewGuid();
            model.password_ = HashHelper.Sha512(model.password_);
        }
        
        if (model.actionType_ == ActionType.Edit.ToInt())
        {
            model.password_ = HashHelper.Sha512(model.newPassword_); 
        }
        
        _con.Execute(sql, new
        {
            model.id_,
            model.account_,
            model.password_,
            model.username_,
            model.phone_,
            model.email_,
            delete_ = 0,
            create_time_ = DateTime.Now,
            modify_time_ = DateTime.Now,
            model.modify_id_
        });
    }

    /// <summary>
    /// 刪除
    /// </summary>
    /// <param name="id"></param>
    public void Delete(Guid id)
    {
        var sql = @"UPDATE a0001_adminAccount SET delete_ = 1 WHERE id_ = @id";
        _con.Execute(sql, new
        {
            id
        });
    }
}