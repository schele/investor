using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Investor.Models.Extensions;
using Investor.SearchTools;
using Investor.UmbExamine;
using Investor.UmbExamine.ExamineSearch;
using Investor.UmbExamine.Model;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace Investor.Controllers.SurfaceControllers
{
    // ToDo Refactor search and package it.
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

    public static class SearchHelper
    {
        public static SearchResult LookForContent(string searchTerm)
        {
            const string searchProviderName = "ExternalSearcher";
            const string indexSet = "ExternalIndexSet";

            UmbracoHelper umbracoHelper = new UmbracoHelper(UmbracoContext.Current);

            var examineSearch = new ExamineSearch(umbracoHelper);

            var fieldFilter = UmbExamineConfig.Instance.GetIndexFields(indexSet, new[] { "umbracoNaviHide" });

            var searchResults = examineSearch.Search(searchTerm, searchProviderName, indexSet, fieldFilter);

            var results = examineSearch.FormatResults<ExamineContentResult>(searchResults, fieldFilter);

            var result = new SearchResult(results, searchResults.SearchTime.TotalSeconds.RoundUp(2));

            return result;
        }

        public static SearchResult LookForImages(string searchTerm)
        {
            const string searchProviderName = "ExternalSearcher";
            const string indexSet = "ExternalIndexSet";

            UmbracoHelper umbracoHelper = new UmbracoHelper(UmbracoContext.Current);

            var examineSearch = new ExamineSearch(umbracoHelper);

            var fieldFilter = UmbExamineConfig.Instance.GetIndexFields(indexSet, new[] { "umbracoNaviHide" });

            var searchResults = examineSearch.Search(searchTerm, searchProviderName, indexSet, fieldFilter);

            var results = examineSearch.FormatResults<ExamineMediaResult>(searchResults, fieldFilter);

            var result = new SearchResult(results, searchResults.SearchTime.TotalSeconds.RoundUp(2));

            return result;
        }

        public static SearchResult LookForFiles(string searchTerm)
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


    public class SearchController : SurfaceController
    {
        [HttpPost]
        public JsonResult LookFor(string searchTerm)
        {
            if (!Validate(searchTerm, out searchTerm))
            {
                return Json(ErrorResult("404", "Search term not found"), JsonRequestBehavior.AllowGet);
            }

            var content = SearchHelper.LookForContent(searchTerm);
            var image = SearchHelper.LookForImages(searchTerm);
            var file = SearchHelper.LookForFiles(searchTerm);

            SearchResult m = SearchHelper.JoinResults(content, image, file);

            // Sort based on name
            m.Result = m.Result.OrderBy(x => x["name"]).ToList();

            var model = new
            {
                searchTime = m.SearchTime,
                results = m.Result
            };

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LookForContent(string searchTerm)
        {
            if (!Validate(searchTerm, out searchTerm))
            {
                return Json(ErrorResult("404", "Search term not found"), JsonRequestBehavior.AllowGet);
            }

            var model = SearchHelper.LookForContent(searchTerm);

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LookForImages(string searchTerm)
        {
            if (!Validate(searchTerm, out searchTerm))
            {
                return Json(ErrorResult("404", "Search term not found"), JsonRequestBehavior.AllowGet);
            }

            var model = SearchHelper.LookForImages(searchTerm);

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LookForFiles(string searchTerm)
        {
            if (!Validate(searchTerm, out searchTerm))
            {
                return Json(ErrorResult("404", "Search term not found"), JsonRequestBehavior.AllowGet);
            }

            var model = SearchHelper.LookForFiles(searchTerm);

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LookForPossibleWords(string searchTerm, string culture)
        {
            const string searchProviderName = "ExternalSearcher";
            const string indexSet = "ExternalIndexSet";
            //const string mediaSearchProviderName = "MediaSearcher";
            //const string mediaIndexSet = "MediaIndexSet";

            //SetUmbracoRequired(culture);

            if (!Validate(searchTerm, out searchTerm))
            {
                return Json(ErrorResult("404", "Search term not found"), JsonRequestBehavior.AllowGet);
            }

            var searchTool = new SearchTool();
            var examineSearch = new ExamineSearch(Umbraco);

            var fieldFilter = UmbExamineConfig.Instance.GetIndexFields(indexSet, new[] { "umbracoNaviHide" });
            //var pdfFieldFilter = UmbExamineConfig.Instance.GetIndexFields(mediaIndexSet);

            var searchResults = examineSearch.Search(searchTerm, searchProviderName, indexSet, fieldFilter);
            //var pdfSearchResults = examineSearch.Search(searchTerm, mediaSearchProviderName, mediaIndexSet, pdfFieldFilter);

            var joinedResult = searchResults.Results;//.Union(pdfSearchResults.Results);

            var result = new List<string>();

            var excludedFields = GetExculdedFields(searchProviderName, fieldFilter)
                //.Union(GetExculdedFields(mediaSearchProviderName, pdfFieldFilter))
                                        .Union(new[] { "__IndexType", "__Path", "__NodeTypeAlias", "__NodeId" })
                                        .ToList();

            foreach (var c in joinedResult)
            {
                foreach (var field in c.Fields)
                {
                    if (excludedFields.Any() && excludedFields.Contains(field.Key))
                    {
                        continue;
                    }

                    searchTool.AddToBuffert(field.Value);
                }
            }

            var matchingWords = searchTool.Search(searchTerm);
            result.AddRange(matchingWords);

            return Json(result.Distinct().ToList(), JsonRequestBehavior.AllowGet);
        }

        Dictionary<string, object> ErrorResult(string errorType, string errorMessage)
        {
            var errorResult = new Dictionary<string, object>();

            errorResult.Add("error", errorType);
            errorResult.Add("errorMessage", errorMessage);

            return errorResult;
        }

        bool Validate(string searchTerm, out string value)
        {
            searchTerm = searchTerm.ToSafeLucineValue();

            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrEmpty(searchTerm))
            {
                value = string.Empty;
                return false;
            }

            value = searchTerm;
            return true;
        }

        bool GetExculdedFields(string searchProviderName, string[] fieldsToFilter, out IEnumerable<string> strings)
        {
            strings = GetExculdedFields(searchProviderName, fieldsToFilter).ToList();
            return strings.Any();
        }

        IEnumerable<string> GetExculdedFields(string searchProviderName, string[] fieldsToFilter)
        {
            var examineConfig = UmbExamineConfig.Instance;

            IEnumerable<string> excludedFields;
            if (examineConfig.ExcludeIndexFields.TryGetValue(searchProviderName, out excludedFields))
            {
                excludedFields = excludedFields.ToList();
                IEnumerable<string> fields = excludedFields;

                return fieldsToFilter.Where(fields.Contains);
            }

            return new List<string>();
        }
    }
}