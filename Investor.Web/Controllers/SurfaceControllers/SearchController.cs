using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Investor.Models.Extensions;
using Investor.SearchTools;
using Investor.UmbExamine;
using Investor.UmbExamine.ExamineSearch;
using Investor.UmbExamine.Model;
using Umbraco.Web.Mvc;

namespace Investor.Controllers.SurfaceControllers
{
    public class SearchController : SurfaceController
    {
        //[HttpPost]
        //public JsonResult LookForProducts(string searchTerm, string culture)
        //{
        //    const string searchProviderName = "ProductSearcher";
        //    const string indexSet = "ProductIndexSet";

        //    if (!Validate(searchTerm, out searchTerm))
        //    {
        //        return Json(ErrorResult("404", "Search term not found"), JsonRequestBehavior.AllowGet);
        //    }

        //    var examineSearch = new ExamineSearch(Umbraco);
        //    var fieldFilter = UmbExamineConfig.Instance.GetIndexFields(indexSet, new[] { "umbracoNaviHide" });

        //    var searchResults = examineSearch.Search(searchTerm, searchProviderName, indexSet, fieldFilter);

        //    var results = examineSearch.FormatResults<ProductResult>(searchResults, fieldFilter);
        //    var searchTime = searchResults.SearchTime.TotalSeconds.RoundUp(2);

        //    var model = new
        //    {
        //        searchTime,
        //        results //= searchResults.Results//.Where(x => x.Fields["nodeTypeAlias"].Equals("BazaarProduct", StringComparison.InvariantCultureIgnoreCase))
        //    };

        //    return Json(model, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult LookFor(string searchTerm)
        {
            const string searchProviderName = "ExternalSearcher";
            const string indexSet = "ExternalIndexSet";

            if (!Validate(searchTerm, out searchTerm))
            {
                return Json(ErrorResult("404", "Search term not found"), JsonRequestBehavior.AllowGet);
            }

            var examineSearch = new ExamineSearch(Umbraco);

            var fieldFilter = UmbExamineConfig.Instance.GetIndexFields(indexSet, new[] { "umbracoNaviHide" });

            var searchResults = examineSearch.Search(searchTerm, searchProviderName, indexSet, fieldFilter);

            var model = new
            {
                searchTime = searchResults.SearchTime.TotalSeconds,
                results = searchResults.Results
            };

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LookForContent(string searchTerm)
        {
            const string searchProviderName = "ExternalSearcher";
            const string indexSet = "ExternalIndexSet";

            if (!Validate(searchTerm, out searchTerm))
            {
                return Json(ErrorResult("404", "Search term not found"), JsonRequestBehavior.AllowGet);
            }

            var examineSearch = new ExamineSearch(Umbraco);

            var fieldFilter = UmbExamineConfig.Instance.GetIndexFields(indexSet, new[] { "umbracoNaviHide" });

            var searchResults = examineSearch.Search(searchTerm, searchProviderName, indexSet, fieldFilter);

            var results = examineSearch.FormatResults<ExamineContentResult>(searchResults, fieldFilter);

            var model = new
            {
                searchTime = searchResults.SearchTime.TotalSeconds.RoundUp(2),
                results
            };

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LookForImages(string searchTerm)
        {
            const string searchProviderName = "ExternalSearcher";
            const string indexSet = "ExternalIndexSet";

            if (!Validate(searchTerm, out searchTerm))
            {
                return Json(ErrorResult("404", "Search term not found"), JsonRequestBehavior.AllowGet);
            }

            var examineSearch = new ExamineSearch(Umbraco);

            var fieldFilter = UmbExamineConfig.Instance.GetIndexFields(indexSet);

            var searchResults = examineSearch.Search(searchTerm, searchProviderName, indexSet, fieldFilter);

            var results = examineSearch.FormatResults<ExamineMediaResult>(searchResults, fieldFilter);

            var model = new
            {
                searchTime = searchResults.SearchTime.TotalSeconds.RoundUp(2),
                results
            };

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LookForFiles(string searchTerm)
        {
            const string mediaSearchProviderName = "MediaSearcher";
            const string mediaIndexSet = "MediaIndexSet";

            // ToDo: adda antal sökta records.

            if (!Validate(searchTerm, out searchTerm))
            {
                return Json(ErrorResult("404", "Search term not found"), JsonRequestBehavior.AllowGet);
            }

            var examineSearch = new ExamineSearch(Umbraco);

            var pdfFieldFilter = UmbExamineConfig.Instance.GetIndexFields(mediaIndexSet);

            var searchResults = examineSearch.Search(searchTerm, mediaSearchProviderName, mediaIndexSet, pdfFieldFilter);

            var results = examineSearch.FormatResults<ExamineFileResult>(searchResults, pdfFieldFilter);

            var model = new
            {
                searchTime = searchResults.SearchTime.TotalSeconds.RoundUp(2),
                results
            };

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