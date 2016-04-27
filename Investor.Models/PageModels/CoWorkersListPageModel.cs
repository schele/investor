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
        DisplayName = "Sida: Listning Medarbetare",
        Icon = Icon.Article,
        Description = "En sida för att lista medarbetare.",
        AllowAtRoot = false,
        AllowedChildNodes = new object[]
            {
                typeof(CoWorkerPageModel)
            }
    )]
    public class CoWorkersListPageModel : BaseModel
    {
        #region constructors

        public CoWorkersListPageModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
        {
        }

        public CoWorkersListPageModel(IPublishedContent content) : base(content)
        {
        }

        #endregion

        #region content

        [Property(
            UmbracoDataType.RichtextEditor,
            Tab.Content,
            DisplayName = "Text",
            Description = "",
            Converter = typeof(RichtextConverter)
        )]
        public virtual IHtmlString Text { get; set; }

        #endregion
    }
}
