using System.Collections.Generic;
using System.Globalization;
using System.Web;
using Investor.Models.Converters;
using Investor.Models.Enums;
using Investor.Models.Models.NodeLink;
using umbraco.interfaces;
using UCodeFirst;
using UCodeFirst.Attributes;
using UCodeFirst.ContentTypes;
using UCodeFirst.Tab;
using Umbraco.Core.Models;

namespace Investor.Models.PageModels
{
    [ContentType(
        DisplayName = "Sida: Start",
        Icon = Icon.Home,
        AllowAtRoot = true,
        Description = "",
        AllowedChildNodes = new object[]
            {
                typeof(PageNotFoundModel),
                typeof(CategoryPageModel),
                typeof(ArticlePageModel),
                typeof(IFramePageModel),
                typeof(SearchPageModel),
                typeof(ContainerPageModel),
                typeof(CoWorkersListPageModel),
                typeof(SitemapPageModel)
            }
    )]
    public class StartPageModel : BaseModel
    {
        #region constructors
        
        public StartPageModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
        {
        }

        public StartPageModel(IPublishedContent content) : base(content)
        {
        }

        #endregion

        #region seo

        [Property(
            UmbracoDataType.Textstring,
            Tab.Seo,
            DisplayName = "Google Analytics",
            Description = "Skriv in en kod f�r Google Analytics"
        )]
        public virtual string GoogleAnalytics { get; set; }

        #endregion

        #region navigation

        [Property(
            UmbracoDataType.ContentPicker,
            Tab.Navigation,
            DisplayName = "L�nk till \"Search\"",
            Description = "",
            Converter = typeof(NodeConverter)
        )]
        public virtual INode SearchNode { get; set; }

        [Property(
            UmbracoDataType.RelatedLinksWithMedia,
            Tab.Navigation,
            DisplayName = "H�gernavigering",
            Description = "",
            Converter = typeof(NodeLinkConverter<NodeLink>)
        )]
        public virtual IEnumerable<NodeLink> AdditionalMenu { get; set; }

        #endregion

        #region footer
 
        [Property(
            UmbracoDataType.ContentPicker,
            Tab.Footer,
            DisplayName = "L�nk till \"About Investor\"",
            Description = "",
            Converter = typeof(NodeConverter)
        )]
        public virtual INode AboutInvestorNode { get; set; }

        [Property(
            UmbracoDataType.ContentPicker,
            Tab.Footer,
            DisplayName = "L�nk till \"Our Investments\"",
            Description = "",
            Converter = typeof(NodeConverter)
        )]
        public virtual INode OurInvestmentsNode { get; set; }

        [Property(
            UmbracoDataType.ContentPicker,
            Tab.Footer,
            DisplayName = "L�nk till \"Investors & Media\"",
            Description = "",
            Converter = typeof(NodeConverter)
        )]
        public virtual INode InvestorsAndMediaNode { get; set; }

        [Property(
            UmbracoDataType.ContentPicker,
            Tab.Footer,
            DisplayName = "L�nk till \"Press Releases\"",
            Description = "",
            Converter = typeof(NodeConverter)
        )]
        public virtual INode PressReleasesNode { get; set; }

        [Property(
            UmbracoDataType.RichtextEditor,
            Tab.Footer,
            DisplayName = "Sidfot",
            Description = "",
            Converter = typeof(RichtextConverter)
        )]
        public virtual IHtmlString Footer { get; set; }

        #endregion

        #region cookie

        [Property(
            UmbracoDataType.TextboxMultiple,
            CustomTabs.Cookie,
            DisplayName = "Cookie: Text",
            Description = ""
        )]
        public virtual string CookieText { get; set; }

        [Property(
            UmbracoDataType.Textstring,
            CustomTabs.Cookie,
            DisplayName = "Cookie: L�nktext",
            Description = ""
        )]
        public virtual string CookieUrlText { get; set; }

        [Property(
            UmbracoDataType.Textstring,
            CustomTabs.Cookie,
            DisplayName = "Cookie: Knapptext",
            Description = ""
        )]
        public virtual string CookieButtonText { get; set; }

        [Property(
            UmbracoDataType.ContentPicker,
            CustomTabs.Cookie,
            DisplayName = "Cookie: L�nk",
            Description = "",
            Converter = typeof(NodeConverter)
        )]
        public virtual INode CookiesNode { get; set; }

        #endregion

        #region content

        [Property(
            "Sida: Start - Grid - Grid layout",
            Tab.Content,
            DisplayName = "Content: Grid",
            Description = ""
        )]
        [MacroCache("Slideshow", "OurInvestments", "PressReleases", "RelatedLinks", "InvestorsAndMedia", "FlexiblePushTextText")]
        public virtual string Grid { get; set; }

        #endregion

        #region popup
        
        [Property(
            UmbracoDataType.TextboxMultiple,
            CustomTabs.Popup,
            DisplayName = "Popup: Text",
            Description = ""
        )]
        public virtual string PopupText { get; set; }

        [Property(
            UmbracoDataType.Textstring,
            CustomTabs.Popup,
            DisplayName = "Popup: L�nktext",
            Description = ""
        )]
        public virtual string PopupUrlText { get; set; }

        [Property(
            UmbracoDataType.SingleRelatedLinksWithMedia,
            CustomTabs.Popup,
            DisplayName = "Popup: L�nk",
            Description = "",
            Converter = typeof(SingleNodeLinkConverter<NodeLink>)
        )]
        public virtual NodeLink PopupUrl { get; set; }

        #endregion
    }
}