using System;
using System.Configuration;
using System.Linq;
using System.Web;
using Investor.Models.PageModels;
using UCodeFirst.Extensions;
using Umbraco.Core;
using Umbraco.Web;

namespace Investor
{
    public class RestrictUmbracoBackofficeBootstraper : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            // In ASP.NET there can by multiple instances of the HttpApplication.
            // The ApplicationInit event will be raised on every HttpApplication.Init()
            UmbracoApplicationBase.ApplicationInit += Init;
        }

        private void Init(object sender, EventArgs eventArgs)
        {
            var app = (HttpApplication)sender;
            app.BeginRequest += UmbracoApplication_BeginRequest;
        }

        private void UmbracoApplication_BeginRequest(object sender, EventArgs e)
        {
            UmbracoApplicationBase application = (UmbracoApplicationBase)sender;
            HttpContext context = application.Context;

            var backofficeAccessAttemt = ConfigurationManager.AppSettings["disable-backoffice-access"].TryConvertTo<bool>();
            var urlPath = context.Request.Url.AbsolutePath.Replace("/#", string.Empty).EnsureStartsWith("/").EnsureEndsWith("/");
            if (backofficeAccessAttemt.Success && backofficeAccessAttemt.Result && (urlPath.StartsWith("/umbraco/login", StringComparison.OrdinalIgnoreCase) || urlPath.Equals("/umbraco/", StringComparison.OrdinalIgnoreCase) || urlPath.StartsWith("/umbraco/Default/", StringComparison.OrdinalIgnoreCase)))
            {
                try
                {
                    var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
                    var roots = ApplicationContext.Current.Services.ContentService.GetRootContent();
                    var startPage = roots.First(x =>
                    {
                        if (x.Published)
                        {
                            var u = new Uri(umbracoHelper.NiceUrlWithDomain(x.Id).EnsureEndsWith("/"), UriKind.Absolute);
                            return context.Request.Url.Host.Equals(u.Host);
                        }

                        return false;
                    });
                    var notFoundPage = startPage.ToModel().Descendants<PageNotFoundModel>().First();
                    context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    context.Response.Cache.SetNoStore();
                    context.Response.Cache.SetExpires(DateTime.MinValue);
                    context.Response.StatusCode = 404;
                    context.Response.Redirect(notFoundPage.Url);
                }
                catch (Exception)
                {
                }

                // If we get here someting went wrong with getting the page specefic 404 page just return plain 404.
                context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                context.Response.Cache.SetNoStore();
                context.Response.Cache.SetExpires(DateTime.MinValue);
                context.Response.StatusCode = 404;
                context.Response.End();
                
                //context.Server.TransferRequest(url, true);
            }
        }
    }
}