using X.PagedList;

namespace ShoppingBackstage.Areas.Account.ViewModels;

public class AdminAccountViewModels
{
    /// <summary>
    /// 唯一識別碼
    /// </summary>
    public Guid id_ { get; set; }

    /// <summary>
    /// 帳號
    /// </summary>
    public string account_ { get; set; }
    
    /// <summary>
    /// 原始密碼
    /// </summary>
    public string password_ { get; set; }
    
    /// <summary>
    /// 新密碼(編輯的時候使用)
    /// </summary>
    public string? newPassword_ { get; set; }
    
    /// <summary>
    /// 使用者名稱
    /// </summary>
    public string username_ { get; set; }

    /// <summary>
    /// 電話
    /// </summary>
    public string? phone_ { get; set; }

    /// <summary>
    /// 信箱
    /// </summary>
    public string email_ { get; set; }
    
    /// <summary>
    /// 創建時間
    /// </summary>
    public DateTime create_time_ { get; set; }
    
    /// <summary>
    /// 最後修改時間
    /// </summary>
    public DateTime modify_time_ { get; set; }
    
    /// <summary>
    /// 修改者
    /// </summary>
    public Guid modify_id_ { get; set; }
    
    /// <summary>
    /// 操作類型
    /// </summary>
    public int actionType_ { get; set; }
}