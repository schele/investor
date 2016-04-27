using System.Globalization;
using UCodeFirst;
using UCodeFirst.ContentTypes;
using UCodeFirst.Tab;
using Umbraco.Core.Models;

namespace Investor.Models.PageModels
{
    [ContentType(
        DisplayName = "Sida: Lista investeringar",
        Icon = Icon.List,
        Description = "En sida för att skapa listning av investeringar.",
        AllowAtRoot = false,
        AllowedChildNodes = new object[]
            {                
                typeof(InvestmentPageModel)
            }
    )]
    public class ListInvestmentsPageModel : BaseModel, IInvestments
    {
        #region constructors

        public ListInvestmentsPageModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
        {
        }

        public ListInvestmentsPageModel(IPublishedContent content) : base(content)
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

        #endregion

        #region push

        [Property(
            UmbracoDataType.TextboxMultiple,
            Tab.Push,
            DisplayName = "Pufftext",
            Description = "Denna text visas i puffar"
        )]
        public virtual string PuffPreamble { get; set; }

        #endregion

        #region page

        [Property(
            "Dropdown for Investments",
            Tab.Page,
            DisplayName = "Dropdown: Rubrikfärg",
            Description = "Välj vilken färg som ska visas för företagsnamnet"
        )]
        public virtual string ColorDropdown { get; set; }

        #endregion
    }
}