using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Investor.Models.Models.NodeLink;
using Investor.Models.PageModels.Interfaces;
using UCodeFirst;
using UCodeFirst.ContentTypes;
using Umbraco.Core.Models;

namespace Investor.Models.PageModels
{
    [ContentType(
    DisplayName = "Sida: Lista presentationer",
    Icon = Icon.List,
    Description = "En sida för att skapa listning av presentationer.",
    AllowAtRoot = false
    )]
    public class ListPresentationsPageModel : BaseModel, IPush
    {
        public ListPresentationsPageModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
        {
        }

        public ListPresentationsPageModel(IPublishedContent content) : base(content)
        {
        }
        public IPublishedContent PushImage
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string PushText
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<NodeLink> RelatedLinksForPush
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string RelatedLinksForPushHeader
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
