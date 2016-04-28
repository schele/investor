using System.Globalization;
using System.Web;
using Investor.Models.Converters;
using Investor.Models.PageModels.Interfaces;
using UCodeFirst;
using UCodeFirst.ContentTypes;
using UCodeFirst.Tab;
using Umbraco.Core.Models;

namespace Investor.Models.PageModels
{
    [ContentType(
        DisplayName = "Sida: Investering",
        Icon = Icon.Company,
        Description = "En sida för att skapa en investering.",
        AllowAtRoot = false
    )]
    public class InvestmentPageModel : BaseModel, IInvestments
    {
        #region constructors
        
        public InvestmentPageModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
        {
        }

        public InvestmentPageModel(IPublishedContent content) : base(content)
        {
        }

        #endregion

        #region page

        [Property(
            "Dropdown for Investments",
            Tab.Page,
            DisplayName = "Dropdown: Rubrikfärg",
            Description = "Välj vilken färg som ska visas för företagsnamnet"
        )]
        public virtual string ColorDropdown { get; set; }

        [Property(
            UmbracoDataType.Textstring,
            Tab.Page,
            DisplayName = "Fakta: Website Header",
            Description = ""
        )]
        public virtual string FactsWebsiteHeader { get; set; }

        [Property(
            UmbracoDataType.Textstring,
            Tab.Page,
            DisplayName = "Fakta: Website",
            Description = ""
        )]
        public virtual string FactsWebsite { get; set; }

        [Property(
            UmbracoDataType.Textstring,
            Tab.Page,
            DisplayName = "Fakta: Rubrik",
            Description = ""
        )]
        public virtual string FactsHeader { get; set; }

        [Property(
            UmbracoDataType.Textstring,
            Tab.Page,
            DisplayName = "Fakta: Office",
            Description = ""
        )]
        public virtual string FactsOffice { get; set; }

        [Property(
            UmbracoDataType.Textstring,
            Tab.Page,
            DisplayName = "Fakta: Listed",
            Description = ""
        )]
        public virtual string FactsListed { get; set; }

        [Property(
            UmbracoDataType.Textstring,
            Tab.Page,
            DisplayName = "Fakta: Business",
            Description = ""
        )]
        public virtual string FactsBusinessArea { get; set; }

        [Property(
            UmbracoDataType.Textstring,
            Tab.Page,
            DisplayName = "Fakta: Share",
            Description = ""
        )]
        public virtual string FactsShare { get; set; }

        [Property(
            UmbracoDataType.Textstring,
            Tab.Page,
            DisplayName = "Fakta: Ownership",
            Description = ""
        )]
        public virtual string FactsOwnership { get; set; }

        [Property(
            UmbracoDataType.Textstring,
            Tab.Page,
            DisplayName = "Fakta: Investment",
            Description = ""
        )]
        public virtual string FactsInvestment { get; set; }

        [Property(
            UmbracoDataType.Textstring,
            Tab.Page,
            DisplayName = "Fakta: Board",
            Description = ""
        )]
        public virtual string FactsBoard { get; set; }

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

        //bugg: när man lägger till en bild blir det fel
        [Property(
            UmbracoDataType.RichtextEditor,
            Tab.Content,
            DisplayName = "Text",
            Description = "",
            Converter = typeof(RichtextConverter)
        )]
        public virtual IHtmlString Text { get; set; }

        [Property(
            UmbracoDataType.MediaPicker,
            Tab.Content,
            DisplayName = "Bild",
            Description = "",
            Converter = typeof(PublishedMediaConverter)
        )]
        public virtual IPublishedContent Image { get; set; }

        #endregion
    }
}