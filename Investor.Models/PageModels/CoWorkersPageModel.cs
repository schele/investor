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
        DisplayName = "Sida: Medarbetare",
        Icon = Icon.Article,
        Description = "En sida för att lista medarbetare.",
        AllowAtRoot = false
    )]
    public class CoWorkersPageModel : BaseModel
    {
        #region constructors

        public CoWorkersPageModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
        {
        }

        public CoWorkersPageModel(IPublishedContent content) : base(content)
        {
        }

        #endregion
    }
}
