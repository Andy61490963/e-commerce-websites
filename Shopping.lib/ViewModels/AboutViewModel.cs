using System.ComponentModel.DataAnnotations;
using X.PagedList;

namespace Shopping.lib.ViewModels;

public class AboutViewModel
{
    public SearchViewModel Search { get; set; }
    
    public IPagedList<AboutManagementViewModel> Abouts { get; set; }
}

public class AboutManagementViewModel
{
    /// <summary>
    /// 編號
    /// </summary>
    [Required]
    public Guid id_ { get; set; }

    /// <summary>
    /// 標題
    /// </summary>
    [Required( ErrorMessage = "需填寫{0}欄位" )]
    [Display( Name = "標題" )]
    [StringLength( 100, ErrorMessage = "{0}長度限{1}" )]
    public string title_ { get; set; }
        
    /// <summary>
    /// 內文
    /// </summary>
    [Required( ErrorMessage = "需填寫{0}欄位" )]
    [Display( Name = "內文" )]
    [StringLength( 500, ErrorMessage = "{0}長度限{1}" )]
    public string content_ { get; set; }
        
    /// <summary>
    /// 尾段
    /// </summary>
    [Required( ErrorMessage = "需填寫{0}欄位" )]
    [Display( Name = "尾段" )]
    [StringLength( 500, ErrorMessage = "{0}長度限{1}" )]
    public string footer_ { get; set; }

    /// <summary>
    /// 啟用
    /// </summary>
    [Required]
    public int enabled_ { get; set; }
    
    /// <summary>
    /// 操作動作
    /// </summary>
    [Required]
    public int ActionType { get; set; }
}
