using System.Reflection;
using UCodeFirst.Converters;
using Umbraco.Web;

namespace Investor.Models.Converters
{
    public class DocumentConverter : IConverter
    {
        public object Read(PropertyInfo propertyInfo, object value)
        {
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);

            return umbracoHelper.TypedMedia(value);
        }
    }
}