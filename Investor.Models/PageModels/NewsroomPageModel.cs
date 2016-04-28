using System.Globalization;
using UCodeFirst;
using UCodeFirst.ContentTypes;
using UCodeFirst.Tab;
using Umbraco.Core.Models;

namespace Investor.Models.PageModels
{
    [ContentType(
        DisplayName = "Sida: Newsroom",
        Icon = Icon.Newspaper,
        Description = "En sida för att skapa ett newsroom",
        AllowAtRoot = true,
        AllowedChildNodes = new object[]
            {                
                typeof(ContainerPageModel)
            }
    )]
    public class NewsroomPageModel : BaseModel
    {
        #region constructors

        public NewsroomPageModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
        {
        }

        public NewsroomPageModel(IPublishedContent content) : base(content)
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
            UmbracoDataType.TextboxMultiple,
            Tab.Content,
            DisplayName = "Ingress",
            Description = ""
        )]
        public virtual string Preamble { get; set; }
        
        #endregion
    }
}