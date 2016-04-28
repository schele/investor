using System.Reflection;
using umbraco.NodeFactory;
using UCodeFirst.Converters;

namespace Investor.Models.Converters
{
    public class NodeConverter : IConverter
    {
        public object Read(PropertyInfo propertyInfo, object value)
        {
            if (value != null)
            {
                if (!string.IsNullOrEmpty(value.ToString()))
                {
                    return new Node(int.Parse(value.ToString()));
                }
            }

            return null;            
        }
    }
}