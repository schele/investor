using System.Collections.Generic;
using System.Globalization;
using Investor.Models.Converters;
using Investor.Models.Models.NodeLink;
using Investor.Models.PageModels.Interfaces;
using umbraco.interfaces;
using UCodeFirst;
using UCodeFirst.ContentTypes;
using UCodeFirst.Factories;
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

        [Property(
            UmbracoDataType.Textstring,
            Tab.Content,
            DisplayName = "Comments",
            Description = "Rubrik för kommentarer"
        )]
        public virtual string CommentsHeader { get; set; }

        [Property(
            UmbracoDataType.Textstring,
            Tab.Content,
            DisplayName = "Notices",
            Description = "Rubrik för notiser"
        )]
        public virtual string NoticesHeader { get; set; }
        
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
            UmbracoDataType.RelatedLinksWithMedia,
            Tab.Push,
            DisplayName = "Puff Relaterade länkar: Länkar",
            Description = "Dessa länkar visas på en puffyta",
            Converter = typeof(NodeLinkConverter<NodeLink>)
        )]
        public virtual IEnumerable<NodeLink> RelatedLinksForPush { get; set; }

        #endregion

        #region navigation

        [Property(
            UmbracoDataType.ContentPicker,
            Tab.Navigation,
            DisplayName = "Länk till \"Kommentarer\"",
            Description = "",
            Converter = typeof(NodeConverter)
        )]
        public virtual INode CommentsNode { get; set; }

        [Property(
            UmbracoDataType.ContentPicker,
            Tab.Navigation,
            DisplayName = "Länk till \"Notiser\"",
            Description = "",
            Converter = typeof(NodeConverter)
        )]
        public virtual INode NoticesNode { get; set; }

        #endregion

        public IEnumerable<CommentPageModel> GetComments()
        {
            var comments = new List<CommentPageModel>();

            foreach (var comment in CommentsNode.ChildrenAsList)
            {
                var commentModel = ModelFactory.Instance.GetModel<CommentPageModel>(comment.Id);

                comments.Add(commentModel);
            }

            return comments;
        }

        public IEnumerable<NoticePageModel> GetNotices()
        {
            var notices = new List<NoticePageModel>();

            foreach (var notice in NoticesNode.ChildrenAsList)
            {
                var noticeModel = ModelFactory.Instance.GetModel<NoticePageModel>(notice.Id);

                notices.Add(noticeModel);
            }

            return notices;
        } 
    }
}