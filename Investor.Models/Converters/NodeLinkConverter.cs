using System.Collections.Generic;
using System.Reflection;
using System.Web.Script.Serialization;
using UCodeFirst.Converters;

namespace Investor.Models.Converters
{    
    public class NodeLinkConverter<T> : IConverter
    {
        public object Read(PropertyInfo propertyInfo, object value)
        {
            var val = value as string;

            if (string.IsNullOrEmpty(val))
            {
                return new List<T>();
            }

            var json = new JavaScriptSerializer();

            return json.Deserialize<List<T>>(val);
        }
    }
}
