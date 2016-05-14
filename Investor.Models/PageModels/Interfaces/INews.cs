using System;
using System.Collections.Generic;
using System.Web;
using Investor.Models.Models.NodeLink;

namespace Investor.Models.PageModels.Interfaces
{
    public interface INews
    {
        string Header { get; set; }

        IHtmlString Preamble { get; set; }

        IEnumerable<NodeLink> RelatedLinks { get; set; }

        DateTime AlternativeDateTime { get; set; }
    }
}