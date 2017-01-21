using System.Globalization;
using System.Linq;
using Examine.LuceneEngine.Config;
using Investor.Models.Extensions;
using Investor.UmbExamine;
using Investor.UmbExamine.ExamineSearch;
using Investor.UmbExamine.Model;
using Umbraco.Web;

namespace Investor.Controllers.SurfaceControllers
{
    public static class SearchHelper
    {
        public static SearchResult LookForContent(string searchTerm, CultureInfo culture = null)
        {
            string searchProviderName = "ExternalSearcher";
            string indexSet = "ExternalIndexSet";

            if (culture != null)
            {
                var cultIndexSet = string.Format("External_{0}_IndexSet", culture.Name);
                var indexItem = IndexSets.Instance.Sets.Cast<IndexSet>().FirstOrDefault(x => x.SetName == cultIndexSet);
                if (indexItem != null)
                {
                    indexSet = cultIndexSet;
                    searchProviderName = string.Format("External_{0}_Searcher", culture.Name);
                }
            }

            UmbracoHelper umbracoHelper = new UmbracoHelper(UmbracoContext.Current);

            var examineSearch = new ExamineSearch(umbracoHelper);

            var fieldFilter = UmbExamineConfig.Instance.GetIndexFields(indexSet, new[] { "umbracoNaviHide" });

            var searchResults = examineSearch.Search(searchTerm, searchProviderName, indexSet, fieldFilter);

            var results = examineSearch.FormatResults<ExamineContentResult>(searchResults, fieldFilter);

            var result = new SearchResult(results, searchResults.SearchTime.TotalSeconds.RoundUp(2));

            return result;
        }

        public static SearchResult LookForImages(string searchTerm, CultureInfo culture = null)
        {
            string searchProviderName = "ExternalSearcher";
            string indexSet = "ExternalIndexSet";

            if (culture != null)
            {
                var cultIndexSet = string.Format("External_{0}_IndexSet", culture.Name);
                var indexItem = IndexSets.Instance.Sets.Cast<IndexSet>().FirstOrDefault(x => x.SetName == cultIndexSet);
                if (indexItem != null)
                {
                    indexSet = cultIndexSet;
                    searchProviderName = string.Format("External_{0}_Searcher", culture.Name);
                }
            }

            UmbracoHelper umbracoHelper = new UmbracoHelper(UmbracoContext.Current);

            var examineSearch = new ExamineSearch(umbracoHelper);

            var fieldFilter = UmbExamineConfig.Instance.GetIndexFields(indexSet, new[] { "umbracoNaviHide" });

            var searchResults = examineSearch.Search(searchTerm, searchProviderName, indexSet, fieldFilter);

            var results = examineSearch.FormatResults<ExamineMediaResult>(searchResults, fieldFilter);

            var result = new SearchResult(results, searchResults.SearchTime.TotalSeconds.RoundUp(2));

            return result;
        }

        public static SearchResult LookForFiles(string searchTerm, CultureInfo culture = null)
        {
            const string mediaSearchProviderName = "MediaSearcher";
            const string mediaIndexSet = "MediaIndexSet";

            // ToDo: adda antal sökta records.

            UmbracoHelper umbracoHelper = new UmbracoHelper(UmbracoContext.Current);

            var examineSearch = new ExamineSearch(umbracoHelper);

            var pdfFieldFilter = UmbExamineConfig.Instance.GetIndexFields(mediaIndexSet);

            var searchResults = examineSearch.Search(searchTerm, mediaSearchProviderName, mediaIndexSet, pdfFieldFilter);

            var results = examineSearch.FormatResults<ExamineFileResult>(searchResults, pdfFieldFilter);

            var result = new SearchResult(results, searchResults.SearchTime.TotalSeconds.RoundUp(2));

            return result;
        }

        public static SearchResult JoinResults(params SearchResult[] searchResults)
        {
            var searchResult = new SearchResult();

            foreach (var result in searchResults)
            {
                searchResult.SearchTime = searchResult.SearchTime + result.SearchTime;
                searchResult.Result.AddRange(result.Result);
            }

            return searchResult;
        }
    }
}