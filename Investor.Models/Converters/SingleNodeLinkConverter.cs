using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Script.Serialization;
using Investor.Models.Models.NodeLink;
using UCodeFirst.Converters;

namespace Investor.Models.Converters
{    
    public class SingleNodeLinkConverter<T> : IConverter
    {
        public object Read(PropertyInfo propertyInfo, object value)
        {
            if (value.Equals(""))
            {
                return null;
            }

            return new JavaScriptSerializer().Deserialize<IEnumerable<NodeLink>>(value.ToString()).First();
        }
    }
}