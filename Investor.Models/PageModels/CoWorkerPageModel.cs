using System.Globalization;
using Investor.Models.Converters;
using UCodeFirst;
using UCodeFirst.ContentTypes;
using UCodeFirst.Tab;
using Umbraco.Core.Models;

namespace Investor.Models.PageModels
{
    [ContentType(
        DisplayName = "Sida: Medarbetare",
        Icon = Icon.User,
        Description = "En sida för att presentera en medarbetare.",
        AllowAtRoot = false
    )]
    public class CoWorkerPageModel : BaseModel
    {
        #region constructors

        public CoWorkerPageModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
        {
        }

        public CoWorkerPageModel(IPublishedContent content) : base(content)
        {
        }

        #endregion

        [Property(
            UmbracoDataType.Textstring,
            Tab.Content,
            DisplayName = "Namn",
            Description = ""
        )]
        public new virtual string Name { get; set; }

        [Property(
            UmbracoDataType.Textstring,
            Tab.Content,
            DisplayName = "Position",
            Description = ""
        )]
        public virtual string Position { get; set; }

        [Property(
            UmbracoDataType.Textstring,
            Tab.Content,
            DisplayName = "Telefon",
            Description = ""
        )]
        public virtual string Phone { get; set; }

        [Property(
            UmbracoDataType.Textstring,
            Tab.Content,
            DisplayName = "Mobil",
            Description = ""
        )]
        public virtual string Mobile { get; set; }

        [Property(
            UmbracoDataType.MediaPicker,
            Tab.Content,
            DisplayName = "Bild",
            Description = "",
            Converter = typeof(PublishedMediaConverter)
        )]
        public virtual IPublishedContent Image { get; set; }
    }
}