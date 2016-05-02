﻿using System.Collections.Generic;
using System.Globalization;
using System.Web;
using Investor.Models.Converters;
using UCodeFirst;
using UCodeFirst.ContentTypes;
using UCodeFirst.Tab;
using Umbraco.Core.Models;

namespace Investor.Models.PageModels
{
    [ContentType(
        DisplayName = "Sida: Kommentar",
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
            UmbracoDataType.RichtextEditor,
            Tab.Content,
            DisplayName = "Text",
            Description = "",
            Converter = typeof(RichtextConverter)
        )]
        public virtual IHtmlString Preamble { get; set; }
        
        [Property(
            UmbracoDataType.MediaPicker,
            Tab.Content,
            DisplayName = "Relaterade dokument: Länkar",
            Description = "",
            Converter = typeof(DocumentConverter)
        )]
        public virtual IEnumerable<IPublishedContent> RelatedDocuments { get; set; }
        
        #endregion
    }
}