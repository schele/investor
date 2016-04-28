using System.Globalization;
using UCodeFirst;
using UCodeFirst.ContentTypes;
using UCodeFirst.Tab;
using Umbraco.Core.Models;

namespace Investor.Models.PageModels
{
    [ContentType(
        DisplayName = "Sida: Comment",
        Icon = Icon.Article,
        Description = "En sida för att skapa en kommentar",
        AllowAtRoot = true,
        AllowedChildNodes = new object[]
            {                
                //typeof(NewsroomPageModel)
            }
    )]
    public class CommentPageModel : BaseModel
    {
        #region constructors

        public CommentPageModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
        {
        }

        public CommentPageModel(IPublishedContent content) : base(content)
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