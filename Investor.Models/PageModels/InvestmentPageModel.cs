using System.Globalization;
using UCodeFirst;
using UCodeFirst.ContentTypes;
using Umbraco.Core.Models;

namespace Investor.Models.PageModels
{
    [ContentType(
        DisplayName = "Sida: Investering",
        Icon = Icon.Company,
        Description = "En sida för att skapa en investering.",
        AllowAtRoot = false
    )]
    public class InvestmentPageModel : BaseModel
    {
        #region constructors
        
        protected InvestmentPageModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
        {
        }

        protected InvestmentPageModel(IPublishedContent content) : base(content)
        {
        }

        #endregion
    }
}