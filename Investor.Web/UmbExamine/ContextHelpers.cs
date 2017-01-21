using System.IO;
using System.Web;
using System.Web.Hosting;
using Umbraco.Core;
using Umbraco.Core.Configuration;
using Umbraco.Web;
using Umbraco.Web.Routing;
using Umbraco.Web.Security;

namespace Investor.UmbExamine
{
    public class ContextHelpers
    {
        public static UmbracoContext EnsureUmbracoContext()
        {
            if (UmbracoContext.Current != null)
            {
                return UmbracoContext.Current;
            }

            return GetSpeceficUmbracoContext("/");

            /* v6.1.4 - v7.2.8 */
            //return UmbracoContext.EnsureContext(dummyHttpContext, ApplicationContext.Current, new WebSecurity(dummyHttpContext, ApplicationContext.Current), false);

            /* v6.1.3 and earlier (I think) */
            //return (typeof(UmbracoContext)
            //    .GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
            //    .First(x => x.GetParameters().Count() == 3)
            //    .Invoke(null, new object[] { dummyHttpContext, ApplicationContext.Current, false })) as UmbracoContext;
        }

        public static UmbracoContext GetSpeceficUmbracoContext(string url)
        {
            var dummyHttpContext = new HttpContextWrapper(new HttpContext(new SimpleWorkerRequest(url, "", new StringWriter())));

            /* v7.3+ */
            return UmbracoContext.EnsureContext(
                dummyHttpContext,
                ApplicationContext.Current,
                new WebSecurity(dummyHttpContext, ApplicationContext.Current),
                UmbracoConfig.For.UmbracoSettings(),
                UrlProviderResolver.Current.Providers,
                false);
        }
    }
}