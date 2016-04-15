using System.Collections.Generic;
using System.Reflection;
using System.Web.Script.Serialization;
using Investor.Models.Models.NodeLink;
using UCodeFirst.Converters;

namespace Investor.Models.Converters
{    
    public class NodeLinkConverter<T> : IConverter
    {
        public object Read(PropertyInfo propertyInfo, object value)
        {
            var links = new JavaScriptSerializer().Deserialize<IEnumerable<NodeLink>>(value.ToString());

            return links;
        }
    }
}