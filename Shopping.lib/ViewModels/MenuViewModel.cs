namespace Shopping.lib.ViewModels;

public class MenuViewModel
{
    /// <summary>
    /// 功能Id
    /// </summary>
    public string id_ { get; set; } = string.Empty;

    /// <summary>
    /// 父功能Id
    /// </summary>
    public string? s0002_id_parent_ { get; set; }

    /// <summary>
    /// 功能名稱
    /// </summary>
    public string name_ { get; set; } = string.Empty;

    /// <summary>
    /// Controller 名稱
    /// </summary>
    public string? controller_ { get; set; }

    /// <summary>
    /// 功能代號
    /// </summary>
    public string? menu_abbreviation_ { get; set; }

    /// <summary>
    /// 功能所屬 area
    /// </summary>
    public string? area_name_ { get; set; }

    /// <summary>
    /// 功能排序
    /// </summary>
    public int sort_ { get; set; }

    /// <summary>
    /// 功能是否共用[沒有授權管控]
    /// </summary>
    public int is_share_ { get; set; }
}