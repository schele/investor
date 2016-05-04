using Examine;
using umbraco;
using Umbraco.Web;

namespace Investor.UmbExamine.Model
{
    public class ExamineContentResult : ResultBase
    {
        public ExamineContentResult(UmbracoHelper helper) : base(helper)
        {
        }

        public override void Formatt(SearchResult result)
        {
            AddProperty("url", library.NiceUrl(result.Id));
            AddProperty("name", result.Fields["nodeName"]);
        }
    }
}
