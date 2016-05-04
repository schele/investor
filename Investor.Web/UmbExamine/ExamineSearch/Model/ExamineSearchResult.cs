using System;
using Examine;
using Investor.UmbExamine.ExamineSearch.Abstraction;

namespace Investor.UmbExamine.ExamineSearch.Model
{
    public class ExamineSearchResult : IExamineSearchResult
    {
        public ISearchResults Results { get; set; }
        public TimeSpan SearchTime { get; set; }

        public ExamineSearchResult()
        {
        }

        public ExamineSearchResult(ISearchResults results, TimeSpan searchTime)
        {
            Results = results;
            SearchTime = searchTime;
        }
    }
}
