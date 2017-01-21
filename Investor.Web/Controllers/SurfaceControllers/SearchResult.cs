using System.Collections.Generic;

namespace Investor.Controllers.SurfaceControllers
{
    public class SearchResult
    {
        public double SearchTime { get; set; }
        public List<Dictionary<string, object>> Result { get; set; }

        public SearchResult()
        {
            Result = new List<Dictionary<string, object>>();
        }

        public SearchResult(List<Dictionary<string, object>> result)
        {
            Result = result;
        }

        public SearchResult(List<Dictionary<string, object>> result, double searchTime)
        {
            Result = result;
            SearchTime = searchTime;
        }
    }
}