using System;
using System.Reflection;
using UCodeFirst.Converters;

namespace Investor.Models.Converters
{    
    public class DateConverter : IConverter
    {
        public object Read(PropertyInfo propertyInfo, object value)
        {
            if (value.Equals(""))
            {
                return null;
            }

            var dateTime = Convert.ToDateTime(value.ToString());

            return dateTime;
        }
    }
}