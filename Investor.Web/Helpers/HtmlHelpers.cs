using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Investor.Extensions;

namespace Investor.Helpers
{
    public static class HtmlHelpers
    {
        public static IHtmlString Render<TObject>(this HtmlHelper helper, TObject o, Func<TObject, HelperResult> itemTemplate = null)
        {
            var buffer = new StringBuilder();
            var writer = new StringWriter(buffer);
            var doWrite = true;
            itemTemplate = itemTemplate ?? (x => new HelperResult(w => w.Write(x)));

            if (!EqualityComparer<TObject>.Default.Equals(o, default(TObject)))
            {
                if (o is string && string.IsNullOrEmpty(o.ChangeType<string>()))
                {
                    doWrite = false;
                }

                if (doWrite)
                {
                    itemTemplate(o).WriteTo(writer);
                }
            }

            return new MvcHtmlString(buffer.ToString());
        }

        public static IHtmlString RenderEnumerable<TObject>(this HtmlHelper helper, TObject o, Func<dynamic, HelperResult> itemTemplate = null)
        {
            var buffer = new StringBuilder();
            var writer = new StringWriter(buffer);
            var doWrite = true;
            itemTemplate = itemTemplate ?? (x => new HelperResult(w => w.Write(x)));

            if (!EqualityComparer<TObject>.Default.Equals(o, default(TObject)))
            {
                if (o is string && string.IsNullOrEmpty(o.ChangeType<string>()))
                {
                    doWrite = false;
                }

                if (doWrite)
                {
                    var enumerable = o as IEnumerable;

                    if (enumerable != null)
                    {
                        var t = enumerable;

                        foreach (var item in t)
                        {
                            itemTemplate(item).WriteTo(writer);
                        }
                    }
                    else
                    {
                        itemTemplate(o).WriteTo(writer);
                    }
                }
            }

            return new MvcHtmlString(buffer.ToString());
        }
    }
}