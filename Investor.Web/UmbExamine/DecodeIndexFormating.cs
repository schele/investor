using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using Examine;
using Examine.LuceneEngine.Providers;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace Investor.UmbExamine
{
    /// <summary>
    /// This will add new Content indexes with decoded values that are specified in UmbExamine settings.
    /// The new indexes is named decoded_propertyAlias
    /// </summary>
    public class DecodeIndexFormating : IApplicationEventHandler
    {
        // Exclude macro container parameters by alias
        public const string ExcludeMacroContainerparameter = "ClassName, macroAlias";

        public void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {

        }

        public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {

        }

        public void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            if (!UmbExamineConfig.Instance.EnableDecodeIndexFormating)
            {
                return;
            }

            var indexer = (LuceneIndexer)ExamineManager.Instance.IndexProviderCollection["ExternalIndexer"];
            indexer.GatheringNodeData += indexer_GatheringNodeData;
        }

        void indexer_GatheringNodeData(object sender, IndexingNodeDataEventArgs e)
        {
            var content = ApplicationContext.Current.Services.ContentService.GetById(e.NodeId);

            // we only want to look into contents skip the rest (media etc)
            if (content == null)
            {
                return;
            }

            for (int index = 0; index < e.Fields.Count; index++)
            {
                var macroContainerExcludedParameter = ExcludeMacroContainerparameter.Split(',');

                var field = e.Fields.ElementAt(index);
                var property = content.Properties.FirstOrDefault(x => x.Alias == field.Key);

                if (property == null)
                {
                    continue;
                }

                // as the property datatypedefinition id and object is hidden by internal we need to get it by reflection...
                var propertyType = (PropertyType)property.GetType()
                                     .GetProperty("PropertyType", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                                     .GetValue(property, null);

                if (UmbExamineConfig.Instance.PropertyEditorsToDecode.Contains(propertyType.PropertyEditorAlias))
                {
                    string decodedValue;

                    if (field.Key == "macroContainer")
                    {
                        //continue;
                        var result = new StringBuilder();

                        var macros = Json.Decode<string[]>(field.Value);

                        foreach (var macro in macros)
                        {
                            // Matching macro parametrar key value
                            var regex = new Regex("(\\w+?)=[\"](.*?)[\"]");

                            var matches = regex.Matches(macro);

                            foreach (Match match in matches)
                            {
                                var macroParameter = (Match)match.Groups[0].Captures[0];

                                var macroParameterName = macroParameter.Groups[1].Value;

                                if (macroContainerExcludedParameter.Any(x => x.Trim().ToLower() == macroParameterName.Trim().ToLower()))
                                {
                                    continue;
                                }

                                var macroParameterContent = macroParameter.Groups[2].Value;

                                int t;
                                if (int.TryParse(macroParameterContent, out t))
                                {
                                    // Ignore all numerics
                                    continue;
                                }

                                macroParameterContent = Uri.UnescapeDataString(macroParameterContent);

                                macroParameterContent = StripHtml(macroParameterContent);

                                result.Append(macroParameterContent + " ");
                            }
                        }

                        decodedValue = result.ToString();

                        decodedValue = HttpUtility.HtmlDecode(decodedValue);

                        e.Fields.Add(string.Format("decoded_{0}", field.Key), decodedValue);

                        // Remove the orginal macro container data
                        e.Fields.Remove(field.Key);

                        continue;
                    }

                    decodedValue = HttpUtility.HtmlDecode(field.Value);

                    e.Fields.Add(string.Format("decoded_{0}", field.Key), decodedValue);
                }
            }
        }

        public static string StripHtml(string htmlString)
        {
            const string pattern = @"<(.|\n)*?>";

            return Regex.Replace(htmlString, pattern, string.Empty);
        }
    }
}