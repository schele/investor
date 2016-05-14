using Examine;
using umbraco;
using Umbraco.Web;

namespace Investor.UmbExamine.Model
{
    public class CoWorkerPageModelContentResult : ResultBase
    {
        public CoWorkerPageModelContentResult(UmbracoHelper helper) : base(helper)
        {
        }

        public override void Format(SearchResult result)
        {
            var position = Umbraco.TypedContent(result.Id).GetPropertyValue("position");
            var phone = Umbraco.TypedContent(result.Id).GetPropertyValue("phone");

            AddProperty("url", library.NiceUrl(result.Id));
            AddProperty("name", result.Fields["nodeName"]);
            AddProperty("text", position + " - " + phone);
            AddProperty("type", "content");
        }
    }
}