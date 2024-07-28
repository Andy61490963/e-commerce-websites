using System.ComponentModel.DataAnnotations;
namespace Shopping.lib.ViewModels;

public class CategoriesViewModels
{
    public SearchViewModel Search { get; set; }
    
    public List<CategoriesManagementViewModels> Categories { get; set; }
}

public class CategoriesManagementViewModels
{
    /// <summary>
    /// 唯一識別碼
    /// </summary>
    public Guid id_ { get; set; }

    /// <summary>
    /// 名稱
    /// </summary>
    [Display( Name = "名稱" )]
    [Required( ErrorMessage = "{0}必填" )]
    [StringLength( 100, ErrorMessage = "{0}不可超過{1}" )]
    public string name_ { get; set; }
    
    /// <summary>
    /// 下拉選單排序
    /// </summary>
    public int? dropdown_order_ { get; set; }
    
    /// <summary>
    /// 圖片排序
    /// </summary>
    public int? layout_order_ { get; set; }
    
    /// <summary>
    /// 是否顯示(開關)
    /// </summary>
    public int enabled_ { get; set; }
    
    /// <summary>
    /// 外觀圖
    /// </summary>
    public Guid s0001_layout_id_ { get; set; }
    
    /// <summary>
    /// 商品banner圖
    /// </summary>
    public Guid s0001_banner_id_ { get; set; }
    
    /// <summary>
    /// 商品外觀圖
    /// </summary>
    public string? LayoutFileObject { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public ServerFileViewModel? LayoutServerFile { get; set; }
    
    /// <summary>
    /// 商品橫幅圖
    /// </summary>
    public string? BannerFileObject { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public ServerFileViewModel? BannerServerFile { get; set; }
    
    /// <summary>
    /// 操作類型
    /// </summary>
    public int actionType_ { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public string? layoutFilesAttach { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public string? bannerFilesAttach { get; set; }
}

public class UpdateOrderRequest
{
    public string Type { get; set; }
    public List<string> Order { get; set; }
}