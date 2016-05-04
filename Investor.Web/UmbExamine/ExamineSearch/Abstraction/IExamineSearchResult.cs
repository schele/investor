using System;
using Examine;

namespace Investor.UmbExamine.ExamineSearch.Abstraction
{
    public interface IExamineSearchResult
    {
        ISearchResults Results { get; set; }
        TimeSpan SearchTime { get; set; }
    }
}
