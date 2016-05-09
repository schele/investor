using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using Examine;
using Examine.LuceneEngine.Providers;
using Skybrud.Umbraco.GridData;
using Skybrud.Umbraco.GridData.Values;
using umbraco;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Web;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Web.Helpers;
using System.Web.Mvc.Routing.Constraints;
using Investor.Controllers.SurfaceControllers;
using Umbraco.Core.Configuration;
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



    /// <summary>
    /// Adds support for indexing and searching in grid data
    /// </summary>
    public class GridIndexFormatting : IApplicationEventHandler
    {
        public void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication,
            ApplicationContext applicationContext)
        {
            
        }

        public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication,
            ApplicationContext applicationContext)
        {

        }

        public void OnApplicationStarted(UmbracoApplicationBase umbracoApplication,
            ApplicationContext applicationContext)
        {
            ExamineManager.Instance.IndexProviderCollection["ExternalIndexer"].GatheringNodeData
                         += (sender, e) => GatheringGridData(sender, e);
        }

        public void GatheringGridData(object sender, IndexingNodeDataEventArgs e)
        {
            try
            {
                var context = ContextHelpers.EnsureUmbracoContext();
                var umbraco = new UmbracoHelper(context);
                var content = umbraco.TypedContent(e.NodeId);
                
                // we only want to look into contents skip the rest (media etc)
                if (content == null)
                {
                    return;
                }

                var contentType = content.ContentType;
                foreach (var prop in content.Properties)
                {
                    var propertyType = contentType.GetPropertyType(prop.PropertyTypeAlias);
                    if (propertyType.PropertyEditorAlias.Equals("Umbraco.Grid"))
                    {
                        GridDataModel grid = GridDataModel.Deserialize(prop.DataValue.ToString());
                        StringBuilder combined = new StringBuilder();

                        foreach (GridControl ctrl in grid.GetAllControls())
                        {

                            switch (ctrl.Editor.Alias)
                            {
                                case "rte":
                                {
                                    // Get the HTML value
                                    string html = ctrl.GetValue<GridControlRichTextValue>().Value;

                                    // Strip any HTML tags so we only have text
                                    string text = Regex.Replace(html, "<.*?>", "");

                                    // Extra decoding may be necessary
                                    text = HttpUtility.HtmlDecode(text);

                                    // Now append the text
                                    combined.AppendLine(text);

                                    break;
                                }
                                case "media":
                                {
                                    GridControlMediaValue media = ctrl.GetValue<GridControlMediaValue>();
                                    combined.AppendLine(media.Caption);
                                    break;
                                }
                                case "headline":
                                case "quote":
                                {
                                    combined.AppendLine(ctrl.GetValue<GridControlTextValue>().Value);
                                    break;
                                }
                                case "macro":
                                {
                                    // Fix certificate problem...
                                    ServicePointManager.ServerCertificateValidationCallback += (senderr, cert, chain, sslPolicyErrors) => true;

                                    var macro = ctrl.GetValue<GridControlMacroValue>();
                                    var p = Json.Encode(macro.Parameters);
                                    // ToDo: Refactor this

                                    // ToDo: create appKey searchHost then add the page path from the content url
                                    //var url = "https://investorab.com.local";// content.Url();
                                    var url =  content.Url();
                                    var host = ConfigurationManager.AppSettings["GridIndexFormatting.host"];

                                    if (!string.IsNullOrEmpty(host))
                                    {
                                        var uri = new Uri(url);
                                        url = uri.Scheme + "://" + host + uri.AbsolutePath;
                                    }

                                    Dictionary<string, object> additionalRouteVals = new Dictionary<string, object>();
                                    additionalRouteVals.Add("alias", macro.MacroAlias);
                                    additionalRouteVals.Add("parameters", p);
                                    
                                    var ufprint = AjaxHelperController.CreateEncryptedRouteString("ajaxhelper", "getmacroresult", "", additionalRouteVals);
                                    WebRequest req = WebRequest.Create(url);
                                    string postData = string.Format("ufprt={0}", ufprint);
                                    
                                    byte[] send = Encoding.Default.GetBytes(postData);
                                    req.Method = "POST";
                                    req.ContentType = "application/x-www-form-urlencoded";
                                    req.ContentLength = send.Length;

                                    Stream sout = req.GetRequestStream();
                                    sout.Write(send, 0, send.Length);
                                    sout.Flush();
                                    sout.Close();

                                    WebResponse res = req.GetResponse();
                                    StreamReader sr = new StreamReader(res.GetResponseStream());
                                    string returnvalue = sr.ReadToEnd();

                                    // Strip any HTML tags so we only have text
                                    string text = Regex.Replace(returnvalue, "<.*?>", "");

                                    // Extra decoding may be necessary
                                    text = HttpUtility.HtmlDecode(text);

                                    // Now append the text
                                    combined.AppendLine(text);

                                    break;
                                }
                            }
                        }

                        e.Fields[prop.PropertyTypeAlias] = combined.ToString();
                    }
                    else
                    {
                        LogHelper.Info<GridIndexFormatting>("Node has no \"content\" value\"");
                    }

                }
            }
            catch (Exception ex)
            {
                LogHelper.Error<GridIndexFormatting>("MAYDAY! MAYDAY! MAYDAY!", ex);
            }
        }

        public static string StripHtml(string htmlString)
        {
            const string pattern = @"<(.|\n)*?>";

            return Regex.Replace(htmlString, pattern, string.Empty);
        }
    }
}