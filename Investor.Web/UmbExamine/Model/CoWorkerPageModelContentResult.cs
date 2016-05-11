using System.Web;
using Examine;
using Investor.Models.Extensions;
using umbraco;
using Umbraco.Web;

namespace Investor.UmbExamine.Model
{
    public class CoWorkerPageModelContentResult : ResultBase
    {
        public CoWorkerPageModelContentResult(UmbracoHelper helper)
            : base(helper)
        {
        }

        public override void Format(SearchResult result)
        {
            AddProperty("url", library.NiceUrl(result.Id));
            AddProperty("name", result.Fields["nodeName"]);
            
            if (result.Fields.ContainsKey("text"))
            {
                var text = result.Fields["text"].GetWords(50); //StripTagsCharArray(result.Fields["text"].TruncateWords(50));

                //Value = "We have a long-term investment perspective and support our companies in their efforts to create sustainable value. Through board participation, industrial experience, our network and financial strength, we strive to make our companies best-in-class.\r\n \r...

                AddProperty("text", HttpUtility.HtmlDecode(text));
            }
            else
            {
                
            }

            AddProperty("type", "content");
        }

        protected string StripTagsCharArray(string html)
        {    
	        var array = new char[html.Length];
	        var arrayIndex = 0;
	        var inside = false;

            for (var i = 0; i < html.Length; i++)
	        {
                char let = html[i];

	            if (let == '<')
	            {
		            inside = true;
		            continue;
	            }

	            if (let == '>')
	            {
		            inside = false;
		            continue;
	            }

	            if (!inside)
	            {
		            array[arrayIndex] = let;
		            arrayIndex++;
	            }
	        }

	        return new string(array, 0, arrayIndex);    
        }
    }
}
