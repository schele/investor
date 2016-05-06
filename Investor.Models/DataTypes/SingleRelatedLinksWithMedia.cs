using UCodeFirst.DataTypes;
using Umbraco.Core.Models;

namespace Investor.Models.DataTypes
{
    [DataType(
        "RG.RelatedLinksWithMedia",
        UmbracoDataType.SingleRelatedLinksWithMedia,
        DataTypeDatabaseType.Ntext
    )]
    public class SingleRelatedLinksWithMedia : DataTypeBase
    {
        public override void UpdateSettings()
        {
            base.UpdateSettings();

            SetPrevalue("max", "1");
            SetPrevalue("hideCaption", "1");
            SetPrevalue("hideNewWindow", "1");
        }
    }
}