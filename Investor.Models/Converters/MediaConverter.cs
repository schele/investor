using System.Reflection;
using Investor.Models.Extensions;

namespace Investor.Models.Converters
{
    public class MediaConverter
    {
        public object Read(PropertyInfo propertyInfo, string value)
        {
            //if (string.IsNullOrEmpty(value))
            //{
            //    var defaultImageId = ObjectFactory.GetInstance<ISiteConstants>().DefaultImageId;

            //    return MediaExtensions.GetMediaById(defaultImageId);
            //}
            
            return MediaExtensions.GetMediaById(value);
        }
    }
}
