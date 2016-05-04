using UCodeFirst.DataTypes;
using Umbraco.Core.Models;

namespace Investor.Models.DataTypes
{
    [DataType("RelatedLinks", UmbracoDataType.RelatedLinksAndMedia, DataTypeDatabaseType.Nvarchar)]
    public class RelatedLinks : DataTypeBase
    {
        public override void UpdateSettings()
        {
            base.UpdateSettings();
        }
    }
}
