using Examine;
using umbraco;
using Umbraco.Web;

namespace Investor.UmbExamine.Model
{
    public class CommentPageModelContentResult : ResultBase
    {
        public CommentPageModelContentResult(UmbracoHelper helper)
            : base(helper)
        {
        }

        public override void Format(SearchResult result)
        {
            var preamble = Umbraco.TypedContent(result.Id).GetPropertyValue("preamble");            

            AddProperty("url", library.NiceUrl(result.Id));
            AddProperty("name", result.Fields["nodeName"]);
            AddProperty("text", preamble);
            AddProperty("type", "content");
        }
    }
}