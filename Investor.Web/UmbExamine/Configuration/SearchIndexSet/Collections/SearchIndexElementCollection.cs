using System.Configuration;

namespace Investor.UmbExamine.Configuration.SearchIndexSet.Collections
{
    [ConfigurationCollection(typeof(SearchIndexElement))]
    public class SearchIndexElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new SearchIndexElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SearchIndexElement)element).Name;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override string ElementName
        {
            get { return "IndexSet"; }
        }

        public SearchIndexElement this[int index]
        {
            get { return (SearchIndexElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }
    }
}