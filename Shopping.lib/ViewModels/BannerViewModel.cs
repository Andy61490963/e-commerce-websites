using System.ComponentModel.DataAnnotations;
using X.PagedList;

namespace Shopping.lib.ViewModels;

public class BannerViewModel
{
    public SearchViewModel Search { get; set; }
    public IPagedList<BannerManagementViewModel> Banners { get; set; }
}

public class BannerManagementViewModel
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
    [StringLength( 50, ErrorMessage = "{0}長度限{1}" )]
    public string caption_ { get; set; }
        
    /// <summary>
    /// 連結網址
    /// </summary>
    [StringLength( 500 )]
    public string? url_ { get; set; }
    
    /// <summary>
    /// 控制水平位置
    /// </summary>
    [Required]
    public int offset_ { get; set; }
        
    /// <summary>
    /// 公告起始日
    /// </summary>
    [Required]
    public DateTime start_date_ { get; set; }

    /// <summary>
    /// 公告截止日
    /// </summary>
    public DateTime? end_date_ { get; set; }

    /// <summary>
    /// 封面Id
    /// </summary>
    public Guid? s0001_id_ { get; set; }

    /// <summary>
    /// 顯示順序
    /// </summary>
    public int order_ { get; set; }

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

    /// <summary>
    /// 
    /// </summary>
    public string? FileObject { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public ServerFileViewModel? ServerFile { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public string? previousFilesAttach { get; set; }
}

