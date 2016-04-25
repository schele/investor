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
        DisplayName = "Sida: Listning Medarbetare",
        Icon = Icon.Article,
        Description = "En sida för att lista medarbetare.",
        AllowAtRoot = false,
        AllowedChildNodes = new object[]
            {
                typeof(CoWorkerPageModel)
            }
    )]
    public class CoWorkersListPageModel : BaseModel
    {
        #region constructors

        public CoWorkersListPageModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
        {
        }

        public CoWorkersListPageModel(IPublishedContent content) : base(content)
        {
        }

        #endregion
    }
}
