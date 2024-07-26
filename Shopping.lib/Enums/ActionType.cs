using System.ComponentModel.DataAnnotations;

namespace Shopping.lib.Enums;

/// <summary>
/// 操作行為
/// </summary>
public enum ActionType
{
    /// <summary>
    /// 新增
    /// </summary>
    [Display( Name = "新增", Description = "新增" )]
    Add,

    /// <summary>
    /// 編輯
    /// </summary>
    [Display( Name = "編輯", Description = "編輯" )]
    Edit,

    /// <summary>
    /// 刪除
    /// </summary>
    [Display( Name = "刪除", Description = "刪除" )]
    Delete,

    /// <summary>
    /// 啟用
    /// </summary>
    [Display( Name = "啟用", Description = "啟用" )]
    Enable,

    /// <summary>
    /// 匯入
    /// </summary>
    [Display( Name = "匯入", Description = "匯入" )]
    Import,

    /// <summary>
    /// 匯出
    /// </summary>
    [Display( Name = "匯出", Description = "匯出" )]
    Export,


    /// <summary>
    /// 授權
    /// </summary>
    [Display(Name = "授權", Description = "授權")]
    Authorize,
    

    /// <summary>
    /// 預覽
    /// </summary>
    [Display(Name = "預覽", Description = "預覽")]
    Preview,

    /// <summary>
    /// 申請完成
    /// </summary>
    [Display(Name = "申請完成", Description = "申請完成")]
    Finish,

    /// <summary>
    /// 審核
    /// </summary>
    [Display(Name = "審核", Description = "審核")]
    Review,
        
    /// <summary>
    /// 複製
    /// </summary>
    [Display(Name = "複製", Description = "複製" )]
    Copy,
    /// <summary>
    /// 檢視
    /// </summary>
    [Display(Name = "檢視", Description = "檢視" )]
    View

}