using Investor.Models.Models.NodeLink.Abstraction;

namespace Investor.Models.Models.NodeLink
{
    public class NodeLink : INodeLink
    {
        public string Caption { get; set; }

        public string Link { get; set; }

        public bool NewWindow { get; set; }

        public bool IsInternal { get; set; }

        public string Internal { get; set; }

        public string Type { get; set; }

        public string Title { get; set; }
    }
}