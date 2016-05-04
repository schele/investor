using System.Collections.Generic;
using Investor.UmbExamine.Model;

namespace Investor.UmbExamine.ExamineSearch.Abstraction
{
    public interface IExamineSearch
    {
        IExamineSearchResult Search(string searchTerm, string searchProviderName, string indexSet, string[] fieldFilter);
        List<Dictionary<string, object>> FormatResults<TResultBase>(IExamineSearchResult searchResults, string[] fieldFilter) where TResultBase : ResultBase;
        List<Dictionary<string, object>> FormatResults(IExamineSearchResult searchResults, string[] fieldFilter);
    }
}
