using Examine;
using Umbraco.Web;

namespace Investor.UmbExamine.Model
{
    public class ExamineMediaResult : ResultBase
    {
        public ExamineMediaResult(UmbracoHelper helper) : base(helper)
        {
        }

        public override void Formatt(SearchResult result)
        {
            AddProperty("name", result.Fields["nodeName"].Replace('_', ' '));

            var img = Umbraco.TypedMedia(result.Id);
            var imageUrl = img.Url;

            if (!string.IsNullOrEmpty(imageUrl))
            {
                this["url"] = imageUrl;
            }
        }
    }
}
