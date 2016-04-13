using System.Globalization;
using UCodeFirst;
using UCodeFirst.ContentTypes;
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
                typeof(PageNotFoundModel)                
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
    }
}