using Examine;
using Umbraco.Web;

namespace Investor.UmbExamine.Model
{
    public class ExamineFileResult : ResultBase
    {
        public ExamineFileResult(UmbracoHelper helper) : base(helper)
        {
        }

        public override void Format(SearchResult result)
        {
            AddProperty("name", result.Fields["nodeName"].Replace('_', ' '));

            var img = Umbraco.TypedMedia(result.Id);

            if (!string.IsNullOrEmpty(img.Url))
            {
                this["url"] = img.Url;
            }

            AddProperty("type", "file");
        }
    }
}
