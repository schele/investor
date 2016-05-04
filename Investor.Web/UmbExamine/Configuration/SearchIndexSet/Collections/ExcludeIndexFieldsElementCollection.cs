using System.Configuration;

namespace Investor.UmbExamine.Configuration.SearchIndexSet.Collections
{
    [ConfigurationCollection(typeof(ExcludeIndexFieldElement))]
    public class ExcludeIndexFieldsElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ExcludeIndexFieldElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var map = ((ExcludeIndexFieldElement)element);

            return map.Name;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override string ElementName
        {
            get { return "add"; }
        }

        public ExcludeIndexFieldElement this[int index]
        {
            get { return (ExcludeIndexFieldElement)BaseGet(index); }
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