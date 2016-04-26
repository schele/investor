using System.Collections.Generic;
using System.Reflection;
using UCodeFirst.Converters;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Investor.Models.Converters
{
    public class DocumentConverter : IConverter
    {
        public object Read(PropertyInfo propertyInfo, object value)
        {
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);

            var images = value.ToString().Split(',');
            var media = new List<IPublishedContent>();

            foreach (var mediaId in images)
            {
                var i = umbracoHelper.TypedMedia(mediaId);

                media.Add(i);
            }

            return media;
        }
    }
}