using System.Collections.Generic;
using System.Globalization;
using Investor.Models.Converters;
using Investor.Models.Models.NodeLink;
using Investor.Models.PageModels.Interfaces;
using UCodeFirst;
using UCodeFirst.ContentTypes;
using UCodeFirst.Tab;
using Umbraco.Core.Models;

namespace Investor.Models.PageModels
{
    [ContentType(
        DisplayName = "Sida: Bildarkiv",
        Icon = Icon.Pictures,
        Description = "En sida för att skapa ett bildarkiv",
        AllowAtRoot = false,
        AllowedChildNodes = new object[]
            {                
                typeof(ImageArchivePageModel)
            }
    )]
    public class ImageArchivePageModel : BaseModel, IPush
    {
        #region constructors

        public ImageArchivePageModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
        {
        }

        public ImageArchivePageModel(IPublishedContent content) : base(content)
        {
        }

        #endregion

        #region puff

        [Property(
            UmbracoDataType.MediaPicker,
            Tab.Push,
            DisplayName = "Puff: Bild",
            Description = "Denna bild visas på en puffyta",
            Converter = typeof(PublishedMediaConverter)
        )]
        public virtual IPublishedContent PushImage { get; set; }

        [Property(
            UmbracoDataType.TextboxMultiple,
            Tab.Push,
            DisplayName = "Puff: Text",
            Description = "Text som visas på en puffyta"
        )]
        public virtual string PushText { get; set; }

        [Property(
            UmbracoDataType.Textstring,
            Tab.Push,
            DisplayName = "Puff Relaterade länkar: Rubrik",
            Description = "Rubrik för relaterade länkar"
        )]
        public virtual string RelatedLinksForPushHeader { get; set; }

        [Property(
            UmbracoDataType.RelatedLinksAndMedia,
            Tab.Push,
            DisplayName = "Puff Relaterade länkar: Länkar",
            Description = "Dessa länkar visas på en puffyta",
            Converter = typeof(NodeLinkConverter<NodeLink>)
        )]
        public virtual IEnumerable<NodeLink> RelatedLinksForPush { get; set; }

        #endregion

        #region content

        [Property(
            "Multiple Media Picker",
            Tab.Content,
            DisplayName = "Bilder",
            Description = "",
            Converter = typeof(DocumentConverter)
        )]
        public virtual IEnumerable<IPublishedContent> Images { get; set; }

        #endregion
    }
}