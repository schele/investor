using System.Globalization;
using System.Web;
using UCodeFirst;
using UCodeFirst.ContentTypes;
using UCodeFirst.Converters;
using UCodeFirst.Tab;
using Umbraco.Core.Models;
using Investor.Models.Converters;

namespace Investor.Models.PageModels
{
    [ContentType(
        DisplayName = "Start",
        Icon = Icon.Home,
        AllowAtRoot = true
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

        #region content
        [Property(
            UmbracoDataType.Textstring,
            Tab.Navigation,
            DisplayName = "Rubrik",
            Description = "H1"
        )]
        public virtual string Header { get; set; }

        [Property(
            UmbracoDataType.RichtextEditor,
            Tab.Navigation,
            DisplayName = "Innehåll",
            Description = "",
            Converter = typeof(RichtextConverter)
        )]
        public virtual IHtmlString Body { get; set; }
        
        [Property(
            UmbracoDataType.MacroContainer,
            Tab.Content,
            DisplayName = "Moduler: topp",
            Description = "Detta är startsidans toppmoduler",
            Converter = typeof(MacroContainerConverter)
        )]
        public virtual IHtmlString StartPageTopModules { get; set; }

        #endregion content
    }
}