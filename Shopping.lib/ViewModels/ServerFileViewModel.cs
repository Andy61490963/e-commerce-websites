namespace Shopping.lib.ViewModels;

public class ServerFileViewModel
{
    /// <summary>
    /// 識別碼
    /// </summary>
    public Guid id_ { get; set; }
    
    /// <summary>
    /// 檔案原始名稱
    /// </summary>
    public string file_name_ { get; set; }
    
    /// <summary>
    /// 檔案顯示名稱
    /// </summary>
    public string display_name_ { get; set; }
    
    /// <summary>
    /// 檔案虛擬路徑
    /// </summary>
    public string file_path_ { get; set; }
    
    /// <summary>
    /// 檔案大小
    /// </summary>
    public string? file_size_ { get; set; }
    
    /// <summary>
    /// 副檔名
    /// </summary>
    public string extension_ { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool delete_ { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime create_time_ { get; set; }
}