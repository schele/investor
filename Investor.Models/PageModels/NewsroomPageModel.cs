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
        DisplayName = "Sida: Newsroom",
        Icon = Icon.Newspaper,
        Description = "En sida för att skapa ett newsroom",
        AllowAtRoot = true,
        AllowedChildNodes = new object[]
            {                
                typeof(ContainerPageModel)
            }
    )]
    public class NewsroomPageModel : BaseModel, IPush
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
            UmbracoDataType.RelatedLinks,
            Tab.Push,
            DisplayName = "Puff Relaterade länkar: Länkar",
            Description = "Dessa länkar visas på en puffyta",
            Converter = typeof(NodeLinkConverter<NodeLink>)
        )]
        public virtual IEnumerable<NodeLink> RelatedLinksForPush { get; set; }

        #endregion
    }
}