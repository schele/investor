using System.Reflection;
using UCodeFirst.Converters;

namespace Investor.Models.Converters
{
    public class BoolConverter : IConverter
    {
        public object Read(PropertyInfo propertyInfo, object value)
        {
            return value;
        }
    }
}