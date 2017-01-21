using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Examine.LuceneEngine.Config;
using Investor.Models.Extensions;
using Investor.SearchTools;
using Investor.UmbExamine;
using SmartFormat;
using Umbraco.Web.Mvc;

namespace Investor.Controllers.SurfaceControllers
{
    // ToDo Refactor search and package it.


    public class SearchController : SurfaceController
    {
        [HttpPost]
        public JsonResult LookFor(string searchTerm)
        {
            if (!Validate(searchTerm, out searchTerm))
            {
                return Json(ErrorResult("404", "Search term not found"), JsonRequestBehavior.AllowGet);
            }

            var culture = Umbraco.CultureDictionary.Culture;
            var content = SearchHelper.LookForContent(searchTerm, culture);
            //var image = SearchHelper.LookForImages(searchTerm);
            var file = SearchHelper.LookForFiles(searchTerm);
            var result = SearchHelper.JoinResults(content, file);

            // Sort based on name
            result.Result = result.Result.OrderBy(x => x["name"]).ToList();

            var messageObject = new
            {
                searchWord = searchTerm,
                resultCount = result.Result.Count
            };

            var messageFormat = Umbraco.GetDictionaryValue(result.Result.Any() ? "System.Search.Result" : "System.Search.ResultEmpty");

            var resultMessage = Smart.Format(messageFormat, messageObject);

            var model = new
            {
                searchTime = result.SearchTime,
                results = result.Result,
                resultMessage
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
        public JsonResult LookForPossibleWords(string searchTerm)
        {
            if (!Validate(searchTerm, out searchTerm))
            {
                return Json(ErrorResult("404", "Search term not found"), JsonRequestBehavior.AllowGet);
            }

            string searchProviderName = "ExternalSearcher";
            string indexSet = "ExternalIndexSet";
            const string mediaSearchProviderName = "MediaSearcher";
            const string mediaIndexSet = "MediaIndexSet";

            var culture = Umbraco.CultureDictionary.Culture;
            if (culture != null)
            {
                var cultIndexSet = string.Format("External_{0}_IndexSet", culture);
                var indexItem = IndexSets.Instance.Sets.Cast<IndexSet>().FirstOrDefault(x => x.SetName == cultIndexSet);
                if (indexItem != null)
                {
                    indexSet = cultIndexSet;
                    searchProviderName = string.Format("External_{0}_Searcher", culture);
                }
            }

            var content = SearchHelper.LookForContent(searchTerm, culture);
            //var image = SearchHelper.LookForImages(searchTerm);
            var file = SearchHelper.LookForFiles(searchTerm);
            var joinedResult = SearchHelper.JoinResults(content, file);

            // Sort based on name
            joinedResult.Result = joinedResult.Result.OrderBy(x => x["name"]).ToList();

            var searchTool = new SearchTool();

            var result = new List<string>();

            var fieldFilter = UmbExamineConfig.Instance.GetIndexFields(indexSet, new[] { "umbracoNaviHide" });
            var pdfFieldFilter = UmbExamineConfig.Instance.GetIndexFields(mediaIndexSet);

            var excludedFields = GetExculdedFields(searchProviderName, fieldFilter)
                .Union(GetExculdedFields(mediaSearchProviderName, pdfFieldFilter))
                                        .Union(new[] { "__IndexType", "__Path", "__NodeTypeAlias", "__NodeId" })
                                        .ToList();

            foreach (var c in joinedResult.Result)
            {
                foreach (var field in c)
                {
                    if (excludedFields.Any() && excludedFields.Contains(field.Key))
                    {
                        continue;
                    }

                    searchTool.AddToBuffert(field.Value.ToString());
                }
            }

            var matchingWords = searchTool.Search(searchTerm);
            result.AddRange(matchingWords);

            return Json(result.Distinct().ToList(), JsonRequestBehavior.AllowGet);

            //string searchProviderName = "ExternalSearcher";
            //string indexSet = "ExternalIndexSet";
            //const string mediaSearchProviderName = "MediaSearcher";
            //const string mediaIndexSet = "MediaIndexSet";

            //if (!Validate(searchTerm, out searchTerm))
            //{
            //    return Json(ErrorResult("404", "Search term not found"), JsonRequestBehavior.AllowGet);
            //}
            
            //var culture = Umbraco.CultureDictionary.Culture;
            //if (culture != null)
            //{
            //    var cultIndexSet = string.Format("External_{0}_IndexSet", culture);
            //    var indexItem = IndexSets.Instance.Sets.Cast<IndexSet>().FirstOrDefault(x => x.SetName == cultIndexSet);
            //    if (indexItem != null)
            //    {
            //        indexSet = cultIndexSet;
            //        searchProviderName = string.Format("External_{0}_Searcher", culture);
            //    }
            //}

            //var searchTool = new SearchTool();
            //var examineSearch = new ExamineSearch(Umbraco);

            //var fieldFilter = UmbExamineConfig.Instance.GetIndexFields(indexSet, new[] { "umbracoNaviHide" });
            //var pdfFieldFilter = UmbExamineConfig.Instance.GetIndexFields(mediaIndexSet);

            //var searchResults = examineSearch.Search(searchTerm, searchProviderName, indexSet, fieldFilter);
            //var pdfSearchResults = examineSearch.Search(searchTerm, mediaSearchProviderName, mediaIndexSet, pdfFieldFilter);

            //var joinedResult = searchResults.Results.Union(pdfSearchResults.Results);

            //var result = new List<string>();

            //var excludedFields = GetExculdedFields(searchProviderName, fieldFilter)
            //    .Union(GetExculdedFields(mediaSearchProviderName, pdfFieldFilter))
            //                            .Union(new[] { "__IndexType", "__Path", "__NodeTypeAlias", "__NodeId" })
            //                            .ToList();

            //foreach (var c in joinedResult)
            //{
            //    foreach (var field in c.Fields)
            //    {
            //        if (excludedFields.Any() && excludedFields.Contains(field.Key))
            //        {
            //            continue;
            //        }

            //        searchTool.AddToBuffert(field.Value);
            //    }
            //}

            //var matchingWords = searchTool.Search(searchTerm);
            //result.AddRange(matchingWords);

            //return Json(result.Distinct().ToList(), JsonRequestBehavior.AllowGet);
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

                if (fieldsToFilter.Any())
                {
                    return fieldsToFilter.Where(fields.Contains);
                }

                return excludedFields;
            }

            return new List<string>();
        }

        public string GetRootUrl(string url)
        {
            var uri = new Uri(url);

            return string.Format("{0}://{1}", uri.Scheme, uri.Authority);
        }
    }
}