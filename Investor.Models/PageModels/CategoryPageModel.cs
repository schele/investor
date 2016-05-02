using System.Globalization;
using UCodeFirst;
using UCodeFirst.ContentTypes;
using UCodeFirst.Tab;
using Umbraco.Core.Models;

namespace Investor.Models.PageModels
{
    [ContentType(
        DisplayName = "Sida: Kategori",
        Icon = Icon.Categories,
        Description = "En sida för att lista kategorier.",
        AllowAtRoot = true,
        AllowedChildNodes = new object[]
            {
                typeof(ListInvestmentsPageModel),
                typeof(IFramePageModel),
                typeof(ArticlePageModel),
                typeof(CoWorkersListPageModel),
                typeof(NewsroomPageModel)
            }
    )]
    public class CategoryPageModel : BaseModel
    {
        #region constructors

        public CategoryPageModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
        {
        }

        public CategoryPageModel(IPublishedContent content) : base(content)
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