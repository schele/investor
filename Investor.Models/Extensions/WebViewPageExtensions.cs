using System.Web.Mvc;

namespace Investor.Models.Extensions
{
    public static class WebViewPageExtensions
    {
        public static bool IsEdit(this WebViewPage viewPage)
        {
            return viewPage.Request.Url != null && viewPage.Request.Url.AbsolutePath.Contains("GetMacroResultAsHtmlForEditor");
        }
    }
}