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
    public class BaseModel : ContentTypeBase
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

        #region seo tab

        [Property(
            UmbracoDataType.Textstring,
            Tab.Seo,
            DisplayName = "Sidans titel",
            Description = "Texten uppe i webbläsarens fönster samt <title> på sidan"
        )]
        public virtual string BrowserTabText { get; set; }

        [Property(
            UmbracoDataType.DropdownForRobots,
            Tab.Seo,
            DisplayName = "Meta: Robots",
            Description = "Meta robots läggs till som ett meta-värde under <meta name=\"robots\">"
        )]
        public virtual string MetaForRobots { get; set; }

        //[Property(
        //    UmbracoDataType.TextboxMultiple,
        //    Tab.Seo,
        //    DisplayName = "Meta: Beskrivning",
        //    Description = "Meta description läggs till som ett meta-värde under <meta name=\"description\">"
        //)]
        //public string MetaDescription { get; set; }

        //[Property(
        //    UmbracoDataType.TextboxMultiple,
        //    Tab.Seo,
        //    DisplayName = "Meta: Nyckelord",
        //    Description = "Meta keywords läggs till som ett meta-värde under <meta name=\"keywords\">"
        //)]
        //public string MetaKeywords { get; set; }

        #endregion

        //public string GetMetaDescription()
        //{
        //    return MetaDescription;
        //}

        public StartPageModel StartPage
        {
            get
            {
                return AncestorOrSelf<StartPageModel>(1);
            }
        }
    }
}
