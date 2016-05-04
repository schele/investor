//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using Hangfire.Dashboard;
//using Umbraco.Core.Security;
//using Umbraco.Web;
//using Umbraco.Web.Mvc;

//namespace MyEcoBuy.Core.Hangfire
//{
//    [UmbracoAuthorize]
//    public class UmbracoAuthorizationFilter : IAuthorizationFilter
//    {
//        public bool Authorize(IDictionary<string, object> owinEnvironment)
//        {
//            // Ensure umbraco user if possible.
//            var http = new HttpContextWrapper(HttpContext.Current);
//            var ticket = http.GetUmbracoAuthTicket();
//            http.AuthenticateCurrentRequest(ticket, true);

//            var user = UmbracoContext.Current.Security.CurrentUser;

//            if (user != null && user.AllowedSections.Contains("developer"))
//            {
//                return true;
//            }

//            return false;
//        }
//    }
//}