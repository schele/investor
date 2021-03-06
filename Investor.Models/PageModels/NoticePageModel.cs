﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
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
        DisplayName = "Sida: Notis",
        Icon = Icon.Article,
        Description = "En sida för att skapa en notis",
        AllowAtRoot = true
    )]
    public class NoticePageModel : BaseModel, INews
    {
        #region constructors

        public NoticePageModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
        {
        }

        public NoticePageModel(IPublishedContent content)
            : base(content)
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

        #endregion

        #region content

        [Property(
            UmbracoDataType.Textstring,
            Tab.Page,
            DisplayName = "Relaterade Länkar: Rubrik",
            Description = ""
        )]
        public virtual string RelatedLinksHeader { get; set; }

        [Property(
            UmbracoDataType.RelatedLinksWithMedia,
            Tab.Page,
            DisplayName = "Relaterade länkar: Länkar",
            Description = "",
            Converter = typeof(NodeLinkConverter<NodeLink>)
        )]
        public virtual IEnumerable<NodeLink> RelatedLinks { get; set; }

        [Property(
            UmbracoDataType.DatePickerWithTime,
            Tab.Page,
            DisplayName = "Datum/Tid",
            Description = "",
            Converter = typeof(DateConverter)
        )]
        public virtual DateTime AlternativeDateTime { get; set; }

        #endregion
    }
}