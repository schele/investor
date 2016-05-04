using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Examine.LuceneEngine.Config;
using Investor.UmbExamine.Configuration;
using Investor.UmbExamine.Configuration.DecodeIndexFormating;
using Investor.UmbExamine.Configuration.SearchIndexSet;

namespace Investor.UmbExamine
{
    public class UmbExamineConfig
    {
        private readonly UmbExamineSection _config = ConfigurationManager.GetSection("UmbExamine") as UmbExamineSection;
        private readonly ConcurrentDictionary<string, IEnumerable<string>> _excludeIndexFields = new ConcurrentDictionary<string, IEnumerable<string>>();
		private string[] _propertyEditorsToDecode = {};
        private static UmbExamineConfig _instance;

        public static UmbExamineConfig Instance
        {
            get { return _instance ?? (_instance = new UmbExamineConfig()); }
            set { _instance = value; }
        }

        public string[] PropertyEditorsToDecode
        {
            get
            {
                if (_config == null)
                {
                    return null;
                }

                if (!_propertyEditorsToDecode.Any())
                {
                    _propertyEditorsToDecode = Enumerable.Cast<PropertyEditorsToDecodeElement>(_config.DecodeIndexFormating.PropertyEditorsToDecode)
                       .Select(x => x.PropertyEditorAlias)
                       .ToArray();
                }

                return _propertyEditorsToDecode;
            }
        }
		
		public IDictionary<string, IEnumerable<string>> ExcludeIndexFields
        {
            get
            {
                if (!_excludeIndexFields.Any())
                {
                    var items = Enumerable.Cast<SearchIndexElement>(_config.SearchIndexSettings)
                        .ToList();

                    for (int i = 0; i < items.Count; i++)
                    {
                        var element = items[i];
                        var excludedFields = Enumerable.Cast<ExcludeIndexFieldElement>(element.ExcludeIndexFields)
                            .Select(x => x.Name);
                        
                        _excludeIndexFields.TryAdd(element.Name, excludedFields);
                    }
                }

                return _excludeIndexFields;
            }
        }

        public bool EnableDecodeIndexFormating
        {
            get
            {
                return _config == null || _config.DecodeIndexFormating.Enabled;
            }
        }

        public bool TryGetIndexFields(string setName, out string[] fields, string[] excludes = null)
        {
            var tryGetIndexSet = Enumerable.Cast<IndexSet>(IndexSets.Instance.Sets).FirstOrDefault(x => x.SetName == setName);

            if (tryGetIndexSet == null)
            {
                fields = null;
                return false;
            }

            var indexAttributeFields = Enumerable.Cast<IndexField>(tryGetIndexSet.IndexAttributeFields).Select(x => x.Name);
            var indexUserFields = Enumerable.Cast<IndexField>(tryGetIndexSet.IndexUserFields).Select(x => x.Name);

            fields = indexAttributeFields.Union(indexUserFields).ToArray();

            if (excludes != null)
            {
                fields = fields.Where(x => !excludes.Contains(x)).ToArray();
            }
            
            return true;
        }

        public string[] GetIndexFields(string setName, string[] excludes = null)
        {
            string[] fields;

            if (!TryGetIndexFields(setName, out fields, excludes))
            {
                throw new Exception(string.Format("IndexSet {0} not found!", setName));
            }

            return fields;
        }

        public IndexSet GetIndexSet(string setName)
        {
            return IndexSets.Instance.Sets[setName];
        }

        
    }
}