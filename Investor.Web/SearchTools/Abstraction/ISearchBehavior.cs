using System.Collections.Generic;

namespace Investor.SearchTools.Abstraction
{
    public interface ISearchBehavior
    {
        IList<string> Buffert { get; set; }
        IList<string> Search(string term);
    }
}
