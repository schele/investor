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
            var links = new NodeLink();

            if (value != null && !value.Equals(""))
            {
                links = new JavaScriptSerializer().Deserialize<NodeLink>(value.ToString());
            }

            return links;
        }
    }
}