using System.Globalization;
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

        #endregion
    }
}