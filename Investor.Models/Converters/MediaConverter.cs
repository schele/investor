using System.Reflection;
using Investor.Models.Extensions;
using UCodeFirst.Converters;

namespace Investor.Models.Converters
{
    public class MediaConverter : IConverter
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
            return MediaExtensions.GetMediaById(value.ToString());
        }
    }
}
