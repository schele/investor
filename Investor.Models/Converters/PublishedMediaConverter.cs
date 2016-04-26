using System.Reflection;
using UCodeFirst.Converters;
using Umbraco.Web;

namespace Investor.Models.Converters
{
    public class PublishedMediaConverter : IConverter
    {
        //public object Read(PropertyInfo propertyInfo, string value)
        //{
            //if (string.IsNullOrEmpty(value))
            //{
            //    var defaultImageId = ObjectFactory.GetInstance<ISiteConstants>().DefaultImageId;

            //    return MediaExtensions.GetMediaById(defaultImageId);
            //}            
        //}

        public object Read(PropertyInfo propertyInfo, object value)
        {
            if (value == null)
            {
                return null;
            }
            
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);

            return umbracoHelper.TypedMedia(value);
        }
    }
}
