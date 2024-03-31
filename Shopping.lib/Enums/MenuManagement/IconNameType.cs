using System.ComponentModel.DataAnnotations;
namespace SA.lib.Enums.MenuManagement
{
    /// <summary>
    /// 圖示 類型
    /// </summary>
    public enum IconNameType
    {
        [Display( Name = "envelope", Description = "信封" )]
        envelope,
        [Display( Name = "watch", Description = "手錶" )]
        watch,
        [Display( Name = "globe", Description = "地球儀" )]
        globe,
        [Display( Name = "handbag", Description = "手提包" )]
        handbag,
        [Display( Name = "heart", Description = "心形" )]
        heart,
        [Display( Name = "house", Description = "房屋" )]
        house,
        [Display( Name = "images", Description = "圖片" )]
        images,
        [Display( Name = "journals", Description = "日記本" )]
        journals,
        [Display( Name = "lamp", Description = "燈" )]
        lamp,
        [Display( Name = "lightbulb", Description = "燈泡" )]
        lightbulb,
        [Display( Name = "magic", Description = "魔法棒" )]
        magic,
        [Display( Name = "map", Description = "地圖" )]
        map,
        [Display( Name = "minecart", Description = "礦車" )]
        minecart,
        [Display( Name = "pencil", Description = "鉛筆" )]
        pencil,
        [Display( Name = "person", Description = "人物" )]
        person,
        [Display( Name = "pin", Description = "圖釘" )]
        pin,
        [Display( Name = "rocket", Description = "火箭" )]
        rocket,
        [Display( Name = "search", Description = "搜尋按鈕" )]
        search,
        [Display( Name = "send", Description = "寄送按鈕" )]
        send,
        [Display( Name = "share", Description = "分享按鈕" )]
        share
    }
}
