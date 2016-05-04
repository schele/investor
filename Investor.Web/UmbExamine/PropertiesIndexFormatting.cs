using Examine;
using Examine.LuceneEngine.Providers;
using Umbraco.Core;

namespace Investor.UmbExamine
{
    /// <summary>
    /// This will add new Content indexes with decoded values that are specified in UmbExamine settings.
    /// The new indexes is named decoded_propertyAlias
    /// </summary>
    public class PropertiesIndexFormatting : IApplicationEventHandler
    {
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

            
        }
    }
}