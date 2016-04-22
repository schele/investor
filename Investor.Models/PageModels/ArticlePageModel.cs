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

        //[Property(
        //    UmbracoDataType.RichtextEditor,
        //    Tab.Content,
        //    DisplayName = "Innehåll",
        //    Description = "",
        //    Converter = typeof(RichtextConverter)
        //)]
        //public virtual IHtmlString Body { get; set; }

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

        //[Property(
        //    UmbracoDataType.MultipleMediaPicker,
        //    Tab.Content,
        //    DisplayName = "Relaterade dokument",
        //    Description = "",
        //    Converter = typeof(MultipleMediaConverter)
        //)]
        //public virtual IEnumerable<IMedia> RelatedDocuments { get; set; }
        
        #endregion

        #region puff

        [Property(
            UmbracoDataType.TextboxMultiple,
            Tab.Push,
            DisplayName = "Puff: Text",
            Description = "Text som visas på en puffyta"
        )]
        public virtual string PushText { get; set; }

        [Property(
            UmbracoDataType.RelatedLinks,
            Tab.Push,
            DisplayName = "Puff: Relaterade länkar",
            Description = "Dessa länkar visas som puffyta",
            Converter = typeof(NodeLinkConverter<NodeLink>)
        )]
        public virtual IEnumerable<NodeLink> RelatedLinksForPush { get; set; }

        //lägg till en bild för puffytan
        //[Property(
        //    UmbracoDataType.RelatedLinks,
        //    Tab.Push,
        //    DisplayName = "Puff: Relaterade länkar",
        //    Description = "Dessa länkar visas som puffyta",
        //    Converter = typeof(NodeLinkConverter<NodeLink>)
        //)]
        //public virtual IEnumerable<NodeLink> ImageForPush { get; set; }

        #endregion
    }
}