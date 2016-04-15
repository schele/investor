using System.Globalization;
using UCodeFirst;
using UCodeFirst.ContentTypes;
using Umbraco.Core.Models;
using UCodeFirst.Tab;

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
                typeof(ArticlePageModel)
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

        [Property(
            UmbracoDataType.Textstring,
            Tab.Seo,
            DisplayName = "Google Analytics",
            Description = "Skriv in en kod för Google Analytics"
        )]
        public virtual string GoogleAnalytics { get; set; }
    }
}