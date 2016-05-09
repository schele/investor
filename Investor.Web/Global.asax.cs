using System;
using System.Web;
using Investor.Configuration;
using Skybrud.Umbraco.GridData;
using StructureMap;
using Umbraco.Web;

namespace Investor
{
    public class Global : UmbracoApplication
    {
        public static HttpContext AppHttpContext;

        protected override void OnApplicationStarting(object sender, EventArgs e)
        {
            base.OnApplicationStarting(sender, e);

            AppHttpContext = HttpContext.Current;

            Bootstrapper.ConfigureStructureMap(ObjectFactory.Container);
            GridPropertyValueConverter.IsEnabled = false;
        }
    }
}