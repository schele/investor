using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Script.Serialization;
using Investor.Models.Models.NodeLink;
using Newtonsoft.Json.Linq;
using UCodeFirst.Converters;

namespace Investor.Models.Converters
{    
    public class NodeLinkConverter<T> : IConverter
    {
        public object Read(PropertyInfo propertyInfo, object value)
        {
            var links = new List<NodeLink>();

            if (value != null && !value.Equals(""))
            {
                links = new JavaScriptSerializer().Deserialize<IEnumerable<NodeLink>>(value.ToString()).ToList();
            }            

            return links;
        }
    }
}