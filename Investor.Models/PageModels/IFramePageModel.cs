using System.Globalization;
using UCodeFirst;
using UCodeFirst.ContentTypes;
using UCodeFirst.Tab;
using Umbraco.Core.Models;

namespace Investor.Models.PageModels
{
    [ContentType(
        DisplayName = "Sida: Iframe",
        Icon = Icon.WindowPopin,
        Description = "En sida för att skapa en iframe.",
        AllowAtRoot = true
    )]
    public class IFramePageModel : BaseModel
    {
        #region constructors

        public IFramePageModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
        {
        }

        public IFramePageModel(IPublishedContent content) : base(content)
        {
        }

        #endregion

        #region content
        
        [Property(
            UmbracoDataType.Textstring,
            Tab.Content,
            DisplayName = "Länk till iframe",
            Description = ""
        )]
        public virtual string IFrameSrc { get; set; }
        
        #endregion
    }
}