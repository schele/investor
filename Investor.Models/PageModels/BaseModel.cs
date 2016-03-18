using System.Globalization;
using UCodeFirst;
using UCodeFirst.ContentTypes;
using UCodeFirst.Tab;
using Umbraco.Core.Models;
using ContentTypeBase = UCodeFirst.ContentTypes.ContentTypeBase;

namespace Investor.Models.PageModels
{
    [ContentType(
        DisplayName = "Base",
        Icon = Icon.Brick
    )]
    public abstract class BaseModel : ContentTypeBase
    {
        #region constructors

        protected BaseModel(IPublishedContent content, CultureInfo culture) : base(content, culture) { }
        protected BaseModel(IPublishedContent content) : base(content) { }

        #endregion

        #region navigation tab

        [Property(
            UmbracoDataType.Textstring,
            Tab.Navigation,
            DisplayName = "Url Alias",
            Description = "Lägg till ytterligare aliases som ska gå till den här sidan."
        )]
        public virtual string UmbracoUrlAlias { get; set; }

        [Property(
            UmbracoDataType.Textstring,
            Tab.Navigation,
            DisplayName = "Url Name",
            Description = "Ändra url adressen som går till den här sidan."
        )]
        public virtual string UmbracoUrlName { get; set; }

        #endregion
    }
}
