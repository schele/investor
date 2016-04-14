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
            UmbracoDataType.RichtextEditor,
            Tab.Content,
            DisplayName = "Innehåll",
            Description = "",
            Converter = typeof(RichtextConverter)
        )]
        public virtual IHtmlString Body { get; set; }

        //External links
        [Property(
            UmbracoDataType.RelatedLinks,
            Tab.Content,
            DisplayName = "Innehåll",
            Description = "",
            Converter = typeof(NodeLinkConverter<NodeLink>)
        )]
        public virtual List<NodeLink> RelatedLinks { get; set; }

        //Documents
        
        #endregion
    }
}