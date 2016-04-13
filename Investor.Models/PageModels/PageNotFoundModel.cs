using System.Globalization;
using System.Web;
using Investor.Models.Converters;
using UCodeFirst;
using UCodeFirst.ContentTypes;
using UCodeFirst.Tab;
using Umbraco.Core.Models;

namespace Investor.Models.PageModels
{
    [ContentType(
        DisplayName = "Sida: 404",
        Icon = Icon.Search,
        Description = "En sida som visas när man går till en länk som inte finns i systemet.",
        AllowAtRoot = true
    )]
    public class PageNotFoundModel : BaseModel
    {
        #region constructors

        public PageNotFoundModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
        {
        }

        public PageNotFoundModel(IPublishedContent content) : base(content)
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
            UmbracoDataType.RichtextEditor,
            Tab.Content,
            DisplayName = "Innehåll",
            Description = "",
            Converter = typeof(RichtextConverter)
        )]
        public virtual IHtmlString Body { get; set; }
        
        #endregion
    }
}
