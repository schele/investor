using System.Configuration;

namespace Investor.UmbExamine.Configuration.DecodeIndexFormating
{
    public class PropertyEditorsToDecodeElement : ConfigurationElement
    {
        public PropertyEditorsToDecodeElement()
        {
        }

        public PropertyEditorsToDecodeElement(string propertyEditorAlias)
        {
            PropertyEditorAlias = propertyEditorAlias;
        }

        [ConfigurationProperty("PropertyEditorAlias", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string PropertyEditorAlias
        {
            get { return (string)this["PropertyEditorAlias"]; }
            set { this["PropertyEditorAlias"] = value; }
        }
    }
}
