using System.Globalization;
using UCodeFirst;
using UCodeFirst.ContentTypes;
using Umbraco.Core.Models;

namespace Investor.Models.PageModels
{
    [ContentType(
        DisplayName = "Container: Standard",
        Icon = Icon.Folder,
        Description = "En sida för att skapa en mapp",
        AllowAtRoot = true,
        AllowedChildNodes = new object[]
            {                
                typeof(CommentPageModel)
            }
    )]
    public class ContainerPageModel : BaseModel
    {
        #region constructors

        public ContainerPageModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
        {
        }

        public ContainerPageModel(IPublishedContent content) : base(content)
        {
        }

        #endregion
    }
}