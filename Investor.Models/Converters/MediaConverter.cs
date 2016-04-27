using System.Reflection;
using Investor.Models.Extensions;
using UCodeFirst.Converters;

namespace Investor.Models.Converters
{
    public class MediaConverter : IConverter
    {
        public object Read(PropertyInfo propertyInfo, object value)
        {
            if (value == null)
            {
                return null;
            }

            return MediaExtensions.GetMediaById(value.ToString());
        }
    }
}