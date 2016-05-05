using System.Collections.Generic;
using System.Globalization;
using System.Web;
using Investor.Models.Converters;
using Investor.Models.Models.NodeLink;
using UCodeFirst;
using UCodeFirst.ContentTypes;
using UCodeFirst.Tab;
using Umbraco.Core.Models;

namespace Investor.Models.PageModels
{
    [ContentType(
        DisplayName = "Sida: Kommentar",
        Icon = Icon.Article,
        Description = "En sida för att skapa en kommentar",
        AllowAtRoot = false
    )]
    public class CommentPageModel : BaseModel
    {
        #region constructors

        public CommentPageModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
        {
        }

        public CommentPageModel(IPublishedContent content) : base(content)
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
            UmbracoDataType.RichtextEditor,
            Tab.Content,
            DisplayName = "Text",
            Description = "",
            Converter = typeof(RichtextConverter)
        )]
        public virtual IHtmlString Preamble { get; set; }

        [Property(
            UmbracoDataType.RelatedLinksWithMedia,
            Tab.Content,
            DisplayName = "Relaterade Länkar: Länkar",
            Description = "",
            Converter = typeof(NodeLinkConverter<NodeLink>)
        )]
        public virtual IEnumerable<NodeLink> RelatedLinks { get; set; }
        
        #endregion
    }
}