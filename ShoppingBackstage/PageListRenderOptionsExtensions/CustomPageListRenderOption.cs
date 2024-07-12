using X.PagedList.Web.Common;

namespace ShoppingBackstage.PageListRenderOptionsExtensions;

public class CustomPageListRenderOption
{
    public static PagedListRenderOptions Ver1
    {
        get
        {
            return new PagedListRenderOptions
            {

                DisplayLinkToFirstPage = PagedListDisplayMode.IfNeeded,
                DisplayLinkToLastPage = PagedListDisplayMode.IfNeeded,
                LinkToPreviousPageFormat = "<i class=\"bi bi-chevron-left\"></i>",
                LinkToNextPageFormat = "<i class=\"bi bi-chevron-right\"></i>",
                DisplayLinkToPreviousPage = PagedListDisplayMode.Always,
                DisplayLinkToNextPage = PagedListDisplayMode.Always,
                DisplayLinkToIndividualPages = true,
                DisplayPageCountAndCurrentLocation = false,
                DisplayEllipsesWhenNotShowingAllPageNumbers = false,
                UlElementClasses = new[] { "pagination justify-content-end" },
                LiElementClasses = new[] { "page-item" },
                PageClasses = new[] { "page-link" },
            };
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="updateTargetId">要更新的Html目標 id</param>
    /// <returns></returns>
    public static PagedListRenderOptions CustomEnableUnobtrusiveAjaxReplacing(string updateTargetId)
    {
        return PagedListRenderOptions.EnableUnobtrusiveAjaxReplacing(Ver1, new AjaxOptions { HttpMethod = "Post", UpdateTargetId = updateTargetId });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ajaxOptions"></param>
    /// <returns></returns>
    public static PagedListRenderOptions CustomEnableUnobtrusiveAjaxReplacing(AjaxOptions ajaxOptions)
    {
        return PagedListRenderOptions.EnableUnobtrusiveAjaxReplacing(Ver1, ajaxOptions);
    }
}