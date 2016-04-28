using System.Collections.Generic;
using Investor.Models.Models.NodeLink;
using Umbraco.Core.Models;

namespace Investor.Models.PageModels.Interfaces
{
    public interface IPush
    {
        IPublishedContent PushImage { get; set; }

        string PushText { get; set; }

        string RelatedLinksForPushHeader { get; set; }

        IEnumerable<NodeLink> RelatedLinksForPush { get; set; }
    }
}