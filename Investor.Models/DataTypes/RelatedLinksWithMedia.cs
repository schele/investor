using UCodeFirst.DataTypes;
using Umbraco.Core.Models;

namespace Investor.Models.DataTypes
{
    //[DataType("RelatedLinks", UmbracoDataType.RelatedLinksAndMedia, DataTypeDatabaseType.Ntext)]
    //public class RelatedLinks : DataTypeBase
    //{
    //    public override void UpdateSettings()
    //    {
    //        base.UpdateSettings();
    //    }
    //}

    [DataType(
        "RG.RelatedLinksWithMedia",
        UmbracoDataType.RelatedLinksWithMedia,
        DataTypeDatabaseType.Ntext
    )]
    public class RelatedLinksWithMedia : DataTypeBase
    {
        public override void UpdateSettings()
        {
            base.UpdateSettings();

            SetPrevalue("max", "100");
        }
    }
}
