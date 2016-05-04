using Investor.SearchTools.Behaviors.FormatFields;
using Investor.SearchTools.Behaviors.Search;

namespace Investor.SearchTools
{
    public class SearchTool : SearchToolsBase
    {
        public SearchTool()
        {
            AddFieldFormater(new NodeUrlFormater());
            SearchBehavior = new DefaultSearch();
        }
    }
}
