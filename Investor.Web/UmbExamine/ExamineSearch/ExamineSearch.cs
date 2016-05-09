using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Examine;
using Investor.UmbExamine.ExamineSearch.Abstraction;
using Investor.UmbExamine.ExamineSearch.Model;
using Investor.UmbExamine.Extensions;
using Investor.UmbExamine.Model;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Umbraco.Web;

namespace Investor.UmbExamine.ExamineSearch
{
    public class ExamineSearch : IExamineSearch
    {
        public UmbracoHelper Umbraco { get; private set; }

        public ExamineSearch(UmbracoHelper umbracoHelper)
        {
            Umbraco = umbracoHelper;
        }

        protected Query WhildCardQuery(string term, string[] fields, Lucene.Net.Util.Version version = null, Analyzer analyzer = null)
        {
            const string whildCardQueryFormat = "{0}:{1}* ";

            if (version == null)
            {
                version = Lucene.Net.Util.Version.LUCENE_29;
            }

            if (analyzer == null)
            {
                analyzer = new StandardAnalyzer(version);
            }

            // Escape
            term = QueryParser.Escape(term);

            var parser = new MultiFieldQueryParser(version, fields, analyzer);
            parser.SetDefaultOperator(QueryParser.OR_OPERATOR);

            var query = new StringBuilder();

            foreach (var field in fields)
            {
                query.AppendFormat(whildCardQueryFormat, field, term);
            }

            var queryString = query.ToString();

            queryString = queryString.Trim();

            var queryResult = parser.Parse(queryString);

            return queryResult;
        }

        protected void AddGeneralProperties<TModel>(IExamineSearchResult examineSearchResult, ref TModel dictionary) where TModel : Dictionary<string, object>
        {
            //dictionary.Add("searchTime", examineSearchResult.SearchTime.TotalSeconds);
        }

        public IExamineSearchResult Search(string searchTerm, string searchProviderName, string indexSet, string[] fieldFilter = null)
        {
            using (var stopWatch = new AutoStopwatch())
            {
                ISearchResults searchResults;

                // If no fields specified trigger a default search
                if (fieldFilter == null || !fieldFilter.Any())
                {
                    searchResults = ExamineManager.Instance
                        .SearchProviderCollection[searchProviderName]
                        .Search(searchTerm, true);

                    return new ExamineSearchResult(searchResults, stopWatch.Elapsed);
                }

                var criteria = ExamineManager.Instance
                    .SearchProviderCollection[searchProviderName]
                    .CreateSearchCriteria();

                // Build our booleanquery that will be a combination of all the queries for each individual search term
                var finalQuery = new BooleanQuery();

                // Split the search string into separate search terms by word
                var terms = searchTerm.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var term in terms)
                {
                    // MultipleCharacterWildcard
                    var wildcardQuery = WhildCardQuery(term, fieldFilter);
                    finalQuery.Add(wildcardQuery, BooleanClause.Occur.SHOULD);
                }

                var lucineQuery = finalQuery.ToString();

                var filter = criteria
                    .RawQuery(lucineQuery);

                searchResults = ExamineManager.Instance
                    .SearchProviderCollection[searchProviderName]
                    .Search(filter);

                return new ExamineSearchResult(searchResults, stopWatch.Elapsed);
            }
        }

        public List<Dictionary<string, object>> FormatResults<TResultBase>(IExamineSearchResult searchResults, string[] fieldFilter) where TResultBase : ResultBase
        {
            var result = new List<Dictionary<string, object>>();

            foreach (SearchResult c in searchResults.Results)
            {
                ResultBase model;

                // ToDo: Factory for ResultBase ?
                string nodeTypeAlias = string.Empty;
                var content = c.TryGetContentOrMedia(Umbraco);

                if (content != null)
                {
                    nodeTypeAlias = content.DocumentTypeAlias;
                }

                var contentType = c.GetContentType(nodeTypeAlias);

                if (contentType == ContentType.Content)
                {
                    model = new ExamineContentResult(Umbraco);
                }
                else if (contentType == ContentType.Media)
                {
                    model = new ExamineMediaResult(Umbraco);
                }
                else if (contentType == ContentType.File)
                {
                    model = new ExamineFileResult(Umbraco);
                }
                else
                {
                    // type not found take all fields in filter.
                    var defaultModel = new Dictionary<string, object>();

                    foreach (var field in c.Fields)
                    {
                        if (fieldFilter.Contains(field.Key))
                        {
                            defaultModel.Add(field.Key, field.Value);
                        }
                    }

                    AddGeneralProperties(searchResults, ref defaultModel);

                    result.Add(defaultModel);

                    continue;
                }

                if (!(model is TResultBase))
                {
                    continue;
                }

                foreach (var field in c.Fields)
                {
                    if (fieldFilter.Contains(field.Key))
                    {
                        model.Add(field.Key, field.Value);
                    }
                }

                AddGeneralProperties(searchResults, ref model);

                // Do formatting for this type
                model.Format(c);

                result.Add(model);
            }

            return result;
        }

        public List<Dictionary<string, object>> FormatResults(IExamineSearchResult searchResults, string[] fieldFilter)
        {
            var result = new List<Dictionary<string, object>>();

            foreach (var c in searchResults.Results)
            {
                ResultBase model;

                // ToDo: Factory for ResultBase ?
                string nodeTypeAlias = string.Empty;
                var content = c.TryGetContentOrMedia(Umbraco);

                if (content != null)
                {
                    nodeTypeAlias = content.DocumentTypeAlias;
                }

                var contentType = c.GetContentType(nodeTypeAlias);

                if (contentType == ContentType.Content)
                {
                    model = new ExamineContentResult(Umbraco);
                }
                else if (contentType == ContentType.Media)
                {
                    model = new ExamineMediaResult(Umbraco);
                }
                else if (contentType == ContentType.File)
                {
                    model = new ExamineFileResult(Umbraco);
                }
                else
                {
                    // type not found take all fields in filter.
                    var defaultModel = new Dictionary<string, object>();

                    foreach (var field in c.Fields)
                    {
                        if (fieldFilter.Contains(field.Key))
                        {
                            defaultModel.Add(field.Key, field.Value);
                        }
                    }

                    AddGeneralProperties(searchResults, ref defaultModel);

                    result.Add(defaultModel);

                    continue;
                }

                foreach (var field in c.Fields)
                {
                    if (fieldFilter.Contains(field.Key))
                    {
                        model.Add(field.Key, field.Value);
                    }
                }

                AddGeneralProperties(searchResults, ref model);

                // Do formatting for this type
                model.Format(c);

                result.Add(model);
            }

            return result;
        }
    }
}