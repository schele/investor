using System.Globalization;
using UCodeFirst;
using UCodeFirst.ContentTypes;
using UCodeFirst.Tab;
using Umbraco.Core.Models;

namespace Investor.Models.PageModels
{
    [ContentType(
        DisplayName = "Sida: Sök",
        Icon = Icon.Search,
        Description = "En sida för att lista sökresultat.",
        AllowAtRoot = true
    )]
    public class SearchPageModel : BaseModel
    {
        #region constructors

        public SearchPageModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
        {
        }

        public SearchPageModel(IPublishedContent content) : base(content)
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