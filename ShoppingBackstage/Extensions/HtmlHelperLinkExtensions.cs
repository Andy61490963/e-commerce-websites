using Microsoft.AspNetCore.Html;
using System.Text;
using System.Text.Encodings.Web;
using System.Web;
using X.PagedList;
using X.PagedList.Mvc.Core;
using X.PagedList.Web.Common;
namespace Microsoft.AspNetCore.Mvc.Rendering
{
    public static class IHtmlHelperExtensions
    {
        public static HtmlString CustomPagedListPager(this IHtmlHelper html, IPagedList list, Func<int, string> generatePageUrl, PagedListRenderOptions options)
        {
            var data = html.PagedListPager(list, generatePageUrl, options).ToString();

            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"d-flex align-items-center \">");
            sb.Append("<div>");
            sb.Append(data);
            sb.Append("</div>");
            sb.Append("<div>");
            sb.Append($"顯示第{list?.FirstItemOnPage ?? 0}筆到第{list?.LastItemOnPage ?? 0}筆，共{list?.TotalItemCount ?? 0}筆");
            sb.Append("</div>");
            sb.Append("</div>");
            return new HtmlString(sb.ToString());
        }

    }
  
}
