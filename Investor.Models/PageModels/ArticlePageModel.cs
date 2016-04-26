using System.Collections.Generic;
using System.Globalization;
using System.Web;
using Investor.Models.Converters;
using Investor.Models.Models.NodeLink;
using UCodeFirst;
using UCodeFirst.Attributes;
using UCodeFirst.ContentTypes;
using UCodeFirst.Tab;
using Umbraco.Core.Models;

namespace Investor.Models.PageModels
{
    [ContentType(
        DisplayName = "Sida: Artikel",
        Icon = Icon.Article,
        Description = "En sida för att skapa en artikel med relaterade länkar och dokument.",
        AllowAtRoot = true,
        AllowedChildNodes = new object[]
            {                
                typeof(ArticlePageModel)
            }
    )]
    public class ArticlePageModel : BaseModel
    {
        #region constructors

        public ArticlePageModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
        {
        }

        public ArticlePageModel(IPublishedContent content) : base(content)
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
            Tab.Page,
            DisplayName = "Relaterade länkar: Rubrik",
            Description = ""
        )]
        public virtual string RelatedLinksHeader { get; set; }

        [Property(
            UmbracoDataType.RelatedLinks,
            Tab.Page,
            DisplayName = "Relaterade länkar: Länkar",
            Description = "",
            Converter = typeof(NodeLinkConverter<NodeLink>)
        )]
        public virtual IEnumerable<NodeLink> RelatedLinks { get; set; }

        [Property(
            "Multiple Media Picker",
            Tab.Page,
            DisplayName = "Relaterade dokument: Dokument",
            Description = "",
            Converter = typeof(DocumentConverter)
        )]
        public virtual IEnumerable<IPublishedContent> RelatedDocuments { get; set; }

        [Property(
            UmbracoDataType.Textstring,
            Tab.Page,
            DisplayName = "Relaterade dokument: Rubrik",
            Description = ""
        )]
        public virtual string RelatedDocumentsHeader { get; set; }
        
        #endregion

        #region puff
        //todo: fungerar inte
        [Property(
            UmbracoDataType.MediaPicker,
            Tab.Push,
            DisplayName = "Puff: Bild",
            Description = "Denna bild visas på en puffyta",
            Converter = typeof(PublishedMediaConverter)
        )]
        public virtual IPublishedContent PushImage { get; set; }

        [Property(
            UmbracoDataType.TextboxMultiple,
            Tab.Push,
            DisplayName = "Puff: Text",
            Description = "Text som visas på en puffyta"
        )]
        public virtual string PushText { get; set; }

        [Property(
            UmbracoDataType.Textstring,
            Tab.Push,
            DisplayName = "Puff Relaterade länkar: Rubrik",
            Description = "Rubrik för relaterade länkar"
        )]
        public virtual string RelatedLinksForPushHeader { get; set; }

        [Property(
            UmbracoDataType.RelatedLinks,
            Tab.Push,
            DisplayName = "Puff Relaterade länkar: Relaterade länkar",
            Description = "Dessa länkar visas på en puffyta",
            Converter = typeof(NodeLinkConverter<NodeLink>)
        )]
        public virtual IEnumerable<NodeLink> RelatedLinksForPush { get; set; }

        #endregion
    }
}