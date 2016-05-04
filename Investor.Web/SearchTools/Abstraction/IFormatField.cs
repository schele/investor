namespace Investor.SearchTools.Abstraction
{
    public interface IFormatField
    {
        string FieldAlias { get; }
        string FormatAlias();
        string FormatField(string value);
    }
}
