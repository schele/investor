using Investor.SearchTools.Abstraction;
using umbraco;

namespace Investor.SearchTools.Behaviors.FormatFields
{
    public class NodeUrlFormater : IFormatField
    {
        public string FieldAlias { get { return "id"; } }

        public string FormatAlias()
        {
            return "nodeUrl";
        }

        public string FormatField(string value)
        {
            return library.NiceUrl(int.Parse(value));
        }
    }
}
