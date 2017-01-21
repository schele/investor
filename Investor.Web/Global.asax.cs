using System;
using System.Configuration;
using System.Web;
using Investor.Configuration;
using ServiceStack.Text;
using Skybrud.Umbraco.GridData;
using StructureMap;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.Routing;

namespace Investor
{
    public class Global : UmbracoApplication
    {
        public static HttpContext AppHttpContext;

        

        public Global()
        {
            //BeginRequest += Global_BeginRequest;
        }

        private void Global_BeginRequest(object sender, EventArgs e)
        {
            var backofficeAccessAttemt = ConfigurationManager.AppSettings["disable-backoffice-access"].TryConvertTo<bool>();
            var urlPath = Request.Url.AbsolutePath.Replace("/#", string.Empty).EnsureStartsWith("/").EnsureEndsWith("/");
            if (backofficeAccessAttemt.Success && backofficeAccessAttemt.Result && (urlPath.StartsWith("/umbraco/login", StringComparison.OrdinalIgnoreCase) || urlPath.Equals("/umbraco/")))
            {
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetNoStore();
                Response.Cache.SetExpires(DateTime.MinValue);
                Response.StatusCode = 404;
                
            }
        }

        protected override void OnApplicationStarting(object sender, EventArgs e)
        {
            base.OnApplicationStarting(sender, e);

            AppHttpContext = HttpContext.Current;

            Bootstrapper.ConfigureStructureMap(ObjectFactory.Container);
            GridPropertyValueConverter.IsEnabled = false;
        }
    }
}