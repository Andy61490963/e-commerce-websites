using System.Text;
using Microsoft.Data.SqlClient;
using Dapper;
using ShoppingBackstage.Areas.Categories.Services.Interface;
using Shopping.lib.ViewModels;

namespace ShoppingBackstage.Areas.Categories.Services.Service;

public class CategoriesManagementService : ICategoriesManagementService
{
    private readonly SqlConnection _con;

    // Constructor
    public CategoriesManagementService(SqlConnection connection)
    {
        _con = connection;
    }

    /// <summary>
    /// 取得 列表
    /// </summary>
    /// <returns></returns>
    public List<CategoriesManagementViewModels> GetList()
    {
        var sql = @"
SELECT 
    c.[id_]
    ,c.[name_]
    ,c.[dropdown_order_]
    ,c.[layout_order_]
    ,c.[enabled_]
    ,c.[s0001_layout_id_]
    ,c.[s0001_banner_id_]
    ,layoutFile.[id_] 
    ,layoutFile.[file_name_]
    ,layoutFile.[file_path_]
    ,layoutFile.[display_name_] 
    ,layoutFile.[extension_] 
    ,bannerFile.[id_] 
    ,bannerFile.[file_name_]
    ,bannerFile.[file_path_] 
    ,bannerFile.[display_name_] 
    ,bannerFile.[extension_] 
FROM [i1001_categories] c
LEFT JOIN s0001_serverFiles layoutFile ON c.s0001_layout_id_ = layoutFile.id_
LEFT JOIN s0001_serverFiles bannerFile ON c.s0001_banner_id_ = bannerFile.id_
WHERE c.[delete_] = 0";

        var categoryDict = new Dictionary<Guid, CategoriesManagementViewModels>();

        var result = _con.Query<CategoriesManagementViewModels, ServerFileViewModel?, ServerFileViewModel?, CategoriesManagementViewModels>(
            sql,
            (category, layoutFile, bannerFile) =>
            {
                if (!categoryDict.TryGetValue(category.id_, out var currentCategory))
                {
                    currentCategory = category;
                    categoryDict.Add(currentCategory.id_, currentCategory);
                }
                
                if (layoutFile != null && layoutFile.id_ != Guid.Empty)
                {
                    currentCategory.LayoutServerFile = layoutFile;
                }

                if (bannerFile != null && bannerFile.id_ != Guid.Empty)
                {
                    currentCategory.BannerServerFile = bannerFile;
                }

                return currentCategory;
            },
            splitOn: "id_,id_" // 依順序性切片
        ).Distinct().ToList();

        return result;
    }
    
    /// <summary>
    /// 取得 單一資料
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public CategoriesManagementViewModels Get(Guid id)
    {
        var sql = @"SELECT * FROM i1001_categories WHERE delete_ = 0 AND id_ = @id";

        var result = _con.QueryFirstOrDefault<CategoriesManagementViewModels>(sql, new { id });

        if (result != null)
        {
            // 查詢附件
            var layoutAttachsql = @"SELECT * FROM s0001_serverFiles WHERE delete_ = 0 AND id_ = @id";
            var layoutAttachRes =
                _con.QueryFirstOrDefault<ServerFileViewModel>(layoutAttachsql, new { id = result.s0001_layout_id_ });

            // 檢查是否有附件資料
            if (layoutAttachRes != null)
            {
                // 將附件資料列表賦值給事件模型的 ServerFile 屬性
                result.LayoutServerFile = layoutAttachRes;
            }

            // 查詢附件
            var baneerAttachsql = @"SELECT * FROM s0001_serverFiles WHERE delete_ = 0 AND id_ = @id";
            var bannerAttachRes =
                _con.QueryFirstOrDefault<ServerFileViewModel>(baneerAttachsql, new { id = result.s0001_banner_id_ });

            // 檢查是否有附件資料
            if (bannerAttachRes != null)
            {
                // 將附件資料列表賦值給事件模型的 ServerFile 屬性
                result.BannerServerFile = bannerAttachRes;
            }
        }

        return result ?? new CategoriesManagementViewModels();
    }

    /// <summary>
    /// 檢查 名稱是否重複
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool IsExists(Guid id, string name)
    {
        var sql = @"SELECT 
                    CASE 
                        WHEN EXISTS (
                            SELECT 1 
                            FROM i1001_categories
                            WHERE name_ = @name
                                AND id_ != @id
			                    AND delete_ = 0
                        ) THEN 1 
                        ELSE 0
                    END AS IsExists
                    ";

        return _con.QueryFirstOrDefault<bool>(sql, new
        {
            name,
            id
        });
    }

    /// <summary>
    /// 新增、更新
    /// </summary>
    /// <param name="model"></param>
    /// <param name="currentUserId"></param>
    public void Save(CategoriesManagementViewModels model, Guid currentUserId)
    {
        var sql = @"MERGE i1001_categories AS target
                    USING (SELECT 
                            @id_ AS id_, 
                            @name_ AS name_, 
                            @dropdown_order_ AS dropdown_order_,
                            @layout_order_ AS layout_order_,
                            @enabled_ AS enabled_,
                            @s0001_layout_id_ AS s0001_layout_id_,
                            @s0001_banner_id_ AS s0001_banner_id_,
                            @delete_ AS delete_, 
                            @create_time_ AS create_time_, 
                            @a0001_id_create_ AS a0001_id_create_, 
                            @a0001_id_modify_ AS a0001_id_modify_, 
                            @modify_time_ AS modify_time_) AS source
                    ON (target.id_ = source.id_)
                    WHEN MATCHED THEN
                        UPDATE SET 
                            name_ = source.name_,
                            dropdown_order_ = source.dropdown_order_,
                            layout_order_ = source.layout_order_,
                            enabled_ = source.enabled_,
                            s0001_layout_id_ = source.s0001_layout_id_,
                            s0001_banner_id_ = source.s0001_banner_id_,
                            modify_time_ = source.modify_time_,
                            a0001_id_modify_ = source.a0001_id_modify_
                    WHEN NOT MATCHED THEN
                        INSERT (id_, name_, dropdown_order_, layout_order_, enabled_, s0001_layout_id_, s0001_banner_id_, delete_, create_time_, a0001_id_create_, a0001_id_modify_, modify_time_)
                        VALUES (source.id_, source.name_, source.dropdown_order_, source.layout_order_, source.enabled_, source.s0001_layout_id_, source.s0001_banner_id_, source.delete_, source.create_time_, source.a0001_id_create_, source.a0001_id_modify_, source.modify_time_);
                ";

        _con.Execute(sql, new
        {
            model.id_,
            model.name_,
            dropdown_order_ = 0,
            layout_order_ = 0,
            model.enabled_,
            s0001_layout_id_ = model.LayoutServerFile?.id_,
            s0001_banner_id_ = model.BannerServerFile?.id_,
            delete_ = 0,
            create_time_ = DateTime.Now,
            modify_time_ = DateTime.Now,
            a0001_id_create_ = currentUserId,
            a0001_id_modify_ = currentUserId
        });
    }


    /// <summary>
    /// 刪除
    /// </summary>
    /// <param name="id"></param>
    public void Delete(Guid id)
    {
        var sql = @"UPDATE i1001_categories SET delete_ = 1 WHERE id_ = @id";
        _con.Execute(sql, new
        {
            id
        });
    }

    /// <summary>
    /// 更新排序
    /// </summary>
    /// <param name="request">包含排序類型和順序的請求對象</param>
    /// <returns></returns>
    public void UpdateOrder(UpdateOrderRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.Type) || request.Order == null)
        {
            throw new ArgumentException("Invalid request data.");
        }

        string columnToUpdate = request.Type == "left" ? "dropdown_order_" : request.Type == "right" ? "layout_order_" : null;

        if (string.IsNullOrEmpty(columnToUpdate))
        {
            throw new ArgumentException("Invalid type specified.");
        }

        var sql = new StringBuilder();
        var parameters = new DynamicParameters();

        for (int i = 0; i < request.Order.Count; i++)
        {
            sql.AppendLine($"UPDATE i1001_categories SET {columnToUpdate} = @order_{i} WHERE id_ = @id_{i};");
            parameters.Add($"@order_{i}", i + 1); // 或者其他排序邏輯
            parameters.Add($"@id_{i}", request.Order[i]);
        }

        _con.Execute(sql.ToString(), parameters);
    }

}