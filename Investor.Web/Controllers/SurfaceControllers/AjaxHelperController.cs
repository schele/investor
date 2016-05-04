using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Investor.Models.Extensions;
using Umbraco.Core;
using Umbraco.Web.Mvc;

namespace Investor.Controllers.SurfaceControllers
{
    public class AjaxHelperController : SurfaceController
    {
        //// ToDo: Add [ValidateAntiForgeryToken] ?
        [HttpPost]
        public JsonResult GetEncryptedRoute(string controllerName, string controllerAction, string area, Dictionary<string, object> additionalRouteVals = null)
        {
            //var s = System.Web.Helpers.Json.Decode(additionalRouteVals);

            var encryptedString = CreateEncryptedRouteString(controllerName, controllerAction, area, additionalRouteVals);

            var model = new Dictionary<string, object>
            {
                {"ufprt", encryptedString}
            };

            return Json(model, JsonRequestBehavior.DenyGet);
        }

        // Från UmbracoHelper.cs
        /// <summary>
        /// This is used in methods like BeginUmbracoForm and SurfaceAction to generate an encrypted string which gets submitted in a request for which
        /// Umbraco can decrypt during the routing process in order to delegate the request to a specific MVC Controller.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="controllerAction"></param>
        /// <param name="area"></param>
        /// <param name="additionalRouteVals"></param>
        /// <returns></returns>
        public static string CreateEncryptedRouteString(string controllerName, string controllerAction, string area, object additionalRouteVals = null)
        {
            //need to create a params string as Base64 to put into our hidden field to use during the routes
            var surfaceRouteParams = string.Format("c={0}&a={1}&ar={2}",
                                                      HttpUtility.UrlEncode(controllerName),
                                                      HttpUtility.UrlEncode(controllerAction),
                                                      area);

            string additionalRouteValsAsQuery;
            if (additionalRouteVals != null)
            {
                if (additionalRouteVals is Dictionary<string, object>)
                {
                    additionalRouteValsAsQuery = ((Dictionary<string, object>)additionalRouteVals).ToQueryString();
                }
                else
                {
                    additionalRouteValsAsQuery = additionalRouteVals.ToDictionary<object>().ToQueryString();
                }
            }
            else
            {
                additionalRouteValsAsQuery = null;
            }

            if (additionalRouteValsAsQuery.IsNullOrWhiteSpace() == false)
                surfaceRouteParams += "&" + additionalRouteValsAsQuery;

            return surfaceRouteParams.EncryptWithMachineKey();
        }
    }
}