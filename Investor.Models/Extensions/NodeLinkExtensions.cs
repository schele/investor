using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Investor.Models.Models.NodeLink;
using Umbraco.Web;

namespace Investor.Models.Extensions
{
    public class NodeLinkEntity
    {
        public string Url { get; set; }
        public string Target { get; set; }
        public readonly NodeLink NodeLink;

        public NodeLinkEntity(NodeLink link)
        {
            NodeLink = link;
        }
    }

    public static class NodeLinkExtensions
    {
        internal const string LinkFormat = "<a href=\"{0}\" title=\"{1}\" target=\"{2}\">{3}</a>";

        public static IHtmlString RelatedLink(this HtmlHelper htmlHelper, NodeLink item, Func<NodeLinkEntity, HelperResult> template = null)
        {
            var helper = new UmbracoHelper(UmbracoContext.Current);
            var entity = new NodeLinkEntity(item);
            template = template ?? (x => new HelperResult(w => w.Write(LinkFormat, entity.Url, item.Caption, entity.Target, item.Caption)));

            if (item.Type == "internal")
            {
                var rel = helper.TypedContent(item.Link);
                entity.Url = rel.Url;

                if (string.IsNullOrEmpty(item.Caption))
                {
                    item.Caption = rel.Name;
                }
            }
            else if (item.Type == "internalMedia")
            {
                var rel = helper.TypedMedia(item.Link);
                entity.Url = rel.Url;

                if (string.IsNullOrEmpty(item.Caption))
                {
                    item.Caption = rel.Name;
                }
            }
            else if (item.Type == "external")
            {
                entity.Url = item.Link;

                if (string.IsNullOrEmpty(item.Caption))
                {
                    item.Caption = item.Link;
                }
            }

            if (item.NewWindow)
            {
                entity.Target = "_blank";
            }

            var buffer = new StringBuilder();
            var writer = new StringWriter(buffer);

            template(entity).WriteTo(writer);

            return new MvcHtmlString(buffer.ToString());
        }

        public static IHtmlString RelatedLinks(this HtmlHelper htmlHelper, IEnumerable<NodeLink> items, Func<NodeLinkEntity, HelperResult> bodyTemplate = null)
        {
            var helper = new UmbracoHelper(UmbracoContext.Current);
            bodyTemplate = bodyTemplate ?? (x => new HelperResult(w => w.Write(htmlHelper.RelatedLink(x.NodeLink))));

            var buffer = new StringBuilder();
            var writer = new StringWriter(buffer);

            foreach (var item in items)
            {
                var entity = new NodeLinkEntity(item);

                if (item.Type == "internal")
                {
                    var rel = helper.TypedContent(item.Link);
                    entity.Url = rel.Url;

                    if (string.IsNullOrEmpty(item.Caption))
                    {
                        item.Caption = rel.Name;
                    }
                }
                else if (item.Type == "internalMedia")
                {
                    var rel = helper.TypedMedia(item.Link);
                    entity.Url = rel.Url;

                    if (string.IsNullOrEmpty(item.Caption))
                    {
                        item.Caption = rel.Name;
                    }
                }
                else if (item.Type == "external")
                {
                    entity.Url = item.Link;

                    if (string.IsNullOrEmpty(item.Caption))
                    {
                        item.Caption = item.Link;
                    }
                }

                if (item.NewWindow)
                {
                    entity.Target = "_blank";
                }

                bodyTemplate(entity).WriteTo(writer);
            }

            return new MvcHtmlString(buffer.ToString());   
        }
    }
}
