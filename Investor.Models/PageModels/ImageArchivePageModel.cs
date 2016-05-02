using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCodeFirst;
using UCodeFirst.ContentTypes;
using Umbraco.Core.Models;

namespace Investor.Models.PageModels
{
    [ContentType(
    DisplayName = "Sida: Bildarkiv",
    Icon = Icon.Users,
    Description = "En sida för att skapa ett bildarkiv",
    AllowAtRoot = false,
    AllowedChildNodes = new object[]
        {
            typeof(ImageArchivePageModel)
        }
    )]
    public class ImageArchivePageModel : BaseModel
    {
        public ImageArchivePageModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
        {
        }

        public ImageArchivePageModel(IPublishedContent content) : base(content)
        {
        }
    }
}
