using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace Investor.Controllers.SurfaceControllers
{
    // url: http://bequia.public.se.local/umbraco/Surface/[ControllerName]/[ActionName]
    // leading wildcard: http://our.umbraco.org/forum/developers/api-questions/12168-Examine-Leading-wildcards?p=1
    public class ConfigSurfaceController : SurfaceController
    {
        //[HttpPost]
        public JsonResult Index()
        {
            var searchQueryKey = ConfigurationManager.AppSettings["searchQueryKey"];

            if (string.IsNullOrEmpty(searchQueryKey))
            {
                searchQueryKey = "s";
            }

            var model = new Dictionary<string, object>
            {
                {"searchQueryKey", searchQueryKey}
            };

            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}