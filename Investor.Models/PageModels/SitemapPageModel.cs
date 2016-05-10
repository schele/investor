using System.Globalization;
using UCodeFirst;
using UCodeFirst.ContentTypes;
using Umbraco.Core.Models;

namespace Investor.Models.PageModels
{
    [ContentType(
        DisplayName = "Sida: Sitemap",
        Icon = Icon.Sitemap,
        Description = "En sida för att skapa en sitemap",
        AllowAtRoot = false
    )]
    public class SitemapPageModel : BaseModel
    {
        #region constructors

        public SitemapPageModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
        {
        }

        public SitemapPageModel(IPublishedContent content) : base(content)
        {
        }

        #endregion
    }
}