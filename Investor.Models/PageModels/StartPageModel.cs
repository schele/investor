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
        DisplayName = "Sida: Start",
        Icon = Icon.Home,
        AllowAtRoot = true,
        Description = "",
        AllowedChildNodes = new object[]
            {
                typeof(PageNotFoundModel),
                typeof(CategoryPageModel),
                typeof(ArticlePageModel),
                typeof(IFramePageModel)
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
            Description = "Skriv in en kod för Google Analytics"
        )]
        public virtual string GoogleAnalytics { get; set; }

        #endregion

        #region navigation

        [Property(
            UmbracoDataType.RelatedLinks,
            Tab.Navigation,
            DisplayName = "Extrameny",
            Description = "",
            Converter = typeof(NodeLinkConverter<NodeLink>)
        )]
        public virtual IEnumerable<NodeLink> AdditionalMenu { get; set; }

        #endregion

        #region footer

        //[Property(
        //    UmbracoDataType.RichtextEditor,
        //    Tab.Footer,
        //    DisplayName = "Sidfot",
        //    Description = "",
        //    Converter = typeof(RichtextConverter)
        //)]
        //public virtual IHtmlString Footer { get; set; }
        
        //[Property(
        //    UmbracoDataType.RichtextEditor,
        //    Tab.Footer,
        //    DisplayName = "Sidfot",
        //    Description = "",
        //    Converter = typeof(RichtextConverter)
        //)]
        //public virtual IHtmlString Footer { get; set; }

       // [Property(
       //    UmbracoDataType.ContentPicker,
       //    Tab.Site,
       //    DisplayName = "About Investor",
       //    Description = "Länk till About Investor",
       //    Converter = typeof(ModelConverter<CategoryPageModel>)
       //)]
       // public virtual CategoryPageModel AboutInvestorPage { get; set; }

        [Property(
            UmbracoDataType.RichtextEditor,
            Tab.Footer,
            DisplayName = "Sidfot",
            Description = "",
            Converter = typeof(RichtextConverter)
        )]
        public virtual IHtmlString Footer { get; set; }
        
        #endregion
    }
}