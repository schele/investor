using System.Configuration;

namespace Investor.UmbExamine.Configuration.SearchIndexSet
{
    public class ExcludeIndexFieldElement : ConfigurationElement
    {
        public ExcludeIndexFieldElement()
        {
        }

        public ExcludeIndexFieldElement(string indexField)
        {
            Name = indexField;
        }

        public override bool IsReadOnly()
        {
            return false;
        }

        [ConfigurationProperty("Name", DefaultValue = "", IsRequired = true)]
        public string Name
        {
            get { return (string)this["Name"]; }
            set { this["Name"] = value; }
        }
    }
}