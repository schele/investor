using System.Web;
using Examine;
using Investor.Models.Extensions;
using umbraco;
using Umbraco.Web;

namespace Investor.UmbExamine.Model
{
    public class ExamineContentResult : ResultBase
    {
        public ExamineContentResult(UmbracoHelper helper) : base(helper)
        {
        }

        public override void Format(SearchResult result)
        {
            AddProperty("url", library.NiceUrl(result.Id));
            AddProperty("name", result.Fields["nodeName"]);
            
            if (result.Fields.ContainsKey("text"))
            {
                var text = result.Fields["text"].TruncateWords(50);
                AddProperty("text", HttpUtility.HtmlDecode(text));
            }

            AddProperty("type", "content");
        }
    }
}
