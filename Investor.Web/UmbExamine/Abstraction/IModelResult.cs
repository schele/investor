namespace Investor.UmbExamine.Abstraction
{
    public interface IModelResult
    {
        //string Name { get; set; }
        //string Url { get; set; }
        //string Type { get; set; }
        //Dictionary<string, object> Model { get; set; }
        void AddProperty(string name, object value);
    }
}
