using System.Configuration;

namespace Investor.UmbExamine.Configuration.DecodeIndexFormating
{
    [ConfigurationCollection(typeof(PropertyEditorsToDecodeElement))]
    public class PropertyEditorsToDecodeElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new PropertyEditorsToDecodeElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PropertyEditorsToDecodeElement)element).PropertyEditorAlias;
        }
    }
}