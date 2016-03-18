using System;
using System.Reflection;
using System.Web;
using HtmlAgilityPack;
using UCodeFirst.Converters;
using Umbraco.Web;

namespace Investor.Models.Converters
{
    public class RichtextConverter : IConverter
    {
        public string FormatImages(string html)
        {
            var doc = new HtmlDocument();

            doc.LoadHtml(html);

            var nodes = doc.DocumentNode.SelectNodes(@"//img[@src]");

            if (nodes == null)
            {
                return html;
            }

            HtmlNode noScriptElement = doc.CreateElement("data-slimmage");

            for (int index = 0; index < nodes.Count; index++)
            {
                var img = nodes[index];
                img.Attributes.Remove("style");

                if (img.Attributes["src"].Value.IndexOf("?", StringComparison.Ordinal) == -1)
                {
                    img.Attributes["src"].Value = img.Attributes["src"].Value + "?width=480&amp;quality=75";
                }

                noScriptElement.AppendChild(img);
                img.ParentNode.InsertAfter(noScriptElement, img);
                nodes.Remove(img);
            }

            var newHtml = doc.DocumentNode.OuterHtml;

            return newHtml;
        }

        public object Read(PropertyInfo propertyInfo, object value)
        {
            var v = value as string;

            if (value is IHtmlString)
            {
                v = FormatImages(((HtmlString)value).ToHtmlString());
            }
            else if (string.IsNullOrEmpty(v) || !UmbracoContext.Current.PageId.HasValue)
            {
                return new HtmlString(string.Empty);
            }

            v = FormatImages(v);

            return new HtmlString(v);
        }
    }
}
