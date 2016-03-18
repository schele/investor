using UCodeFirst.DataTypes;
using Umbraco.Core.Models;

namespace Investor.Models.DataTypes
{
    [DataType(
        "MacroContainerEditor",
        UmbracoDataType.MacroContainer,
        DataTypeDatabaseType.Ntext
    )]
    public class MacroContainer : DataTypeBase
    {
    }
}