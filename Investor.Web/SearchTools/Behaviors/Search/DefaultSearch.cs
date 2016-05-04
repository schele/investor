using System.Collections.Generic;
using Investor.SearchTools.Abstraction;

namespace Investor.SearchTools.Behaviors.Search
{
    public class DefaultSearch : ISearchBehavior
    {
        public IList<string> Buffert { get; set; }

        public DefaultSearch()
        {
            Buffert = new List<string>();
        }
        
        public IList<string> Search(string term)
        {
            var matches = new List<string>();

            foreach (var value in Buffert)
            {
                //var decodedValue = HttpUtility.HtmlDecode(value);

                using (var filterSearch = new FilterSearchInputString(value))
                {
                    matches.AddRange(filterSearch.FilterMatches(term));
                }
            }
            
            return matches;
        }
    }
}
