using System.Configuration;
using Investor.UmbExamine.Configuration.SearchIndexSet.Collections;

namespace Investor.UmbExamine.Configuration.SearchIndexSet
{
    public class SearchIndexElement : ConfigurationElement
    {
        [ConfigurationProperty("Name", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this["Name"]; }
            set { this["Name"] = value; }
        }

        [ConfigurationProperty("ExcludeIndexFields")]
        public ExcludeIndexFieldsElementCollection ExcludeIndexFields
        {
            get { return (ExcludeIndexFieldsElementCollection)this["ExcludeIndexFields"]; }
            set { this["ExcludeIndexFields"] = value; }
        }
    }
}
