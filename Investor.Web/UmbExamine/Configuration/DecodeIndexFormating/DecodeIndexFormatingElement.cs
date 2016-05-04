using System.Configuration;

namespace Investor.UmbExamine.Configuration.DecodeIndexFormating
{
    public class DecodeIndexFormatingElement : ConfigurationElement
    {
        /// <summary>
        /// Is enabled
        /// </summary>
        [ConfigurationProperty("Enabled", DefaultValue = true)]
        public bool Enabled
        {
            get { return (bool)this["Enabled"]; }
            set { this["Enabled"] = value; }
        }

        [ConfigurationProperty("PropertyEditorsToDecode", IsDefaultCollection = true)]
        public PropertyEditorsToDecodeElementCollection PropertyEditorsToDecode
        {
            get { return (PropertyEditorsToDecodeElementCollection)this["PropertyEditorsToDecode"]; }
            set { this["PropertyEditorsToDecode"] = value; }
        }
    }
}