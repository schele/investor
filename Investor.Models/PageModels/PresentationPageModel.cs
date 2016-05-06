using System.Collections.Generic;
using System.Globalization;
using Investor.Models.Converters;
using Investor.Models.Models.NodeLink;
using UCodeFirst;
using UCodeFirst.ContentTypes;
using UCodeFirst.Tab;
using Umbraco.Core.Models;

namespace Investor.Models.PageModels
{
    [ContentType(
        DisplayName = "Sida: Presentation",
        Icon = Icon.Presentation,
        Description = "En sida för att lista presentationer",
        AllowAtRoot = false
    )]
    public class PresentationPageModel : BaseModel
    {
        #region constructors

        public PresentationPageModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
        {
        }

        public PresentationPageModel(IPublishedContent content) : base(content)
        {
        }

        #endregion

        #region content
     
        [Property(
            UmbracoDataType.Textstring,
            Tab.Content,
            DisplayName = "Rubrik",
            Description = "H1"
        )]
        public virtual string Header { get; set; }

        [Property(
            UmbracoDataType.TextboxMultiple,
            Tab.Content,
            DisplayName = "Ingress",
            Description = ""
        )]
        public virtual string Preamble { get; set; }

        [Property(
            UmbracoDataType.Textstring,
            Tab.Content,
            DisplayName = "Länkar: Rubrik",
            Description = ""
        )]
        public virtual string RelatedLinksHeader { get; set; }

        [Property(
            UmbracoDataType.RelatedLinksWithMedia,
            Tab.Content,
            DisplayName = "Länkar: Länkar",
            Description = "",
            Converter = typeof(NodeLinkConverter<NodeLink>)
        )]
        public virtual IEnumerable<NodeLink> RelatedLinks { get; set; }
        
        #endregion        
    }
}