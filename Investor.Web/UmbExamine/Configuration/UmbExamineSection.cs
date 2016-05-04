using System.Configuration;
using Investor.UmbExamine.Configuration.DecodeIndexFormating;
using Investor.UmbExamine.Configuration.SearchIndexSet.Collections;

/*
<!-- SECTION DECLARATION -->
<section name="UmbExamine" type="Oscarclean.Public.Cms.UmbExamine.Configuration.UmbExamineSection, Oscarclean.Public.Cms" />
<! -- CONFIGURATION -->
<UmbExamine>
    <DecodeIndexFormating Enabled="True|False">
        <PropertyEditorsToDecode>
            <add PropertyEditorAlias="Umbraco.TinyMCEv3" />
        </PropertyEditorsToDecode>
    </DecodeIndexFormating>
  <SearchIndexSettings>
    <IndexSet Name="ExternalSearcher">
      <ExcludeIndexFields>
        <add Name="id" />
        <add Name="nodeTypeAlias" />
        <add Name="parentID" />
        </ExcludeIndexFields>
    </IndexSet>
  </SearchIndexSettings>
</UmbExamine>
*/

namespace Investor.UmbExamine.Configuration
{
    public class UmbExamineSection : ConfigurationSection
    {
        /// <summary>
        /// Settings for the Decoder Index
        /// </summary>
        [ConfigurationProperty("DecodeIndexFormating", IsRequired = false)]
        public DecodeIndexFormatingElement DecodeIndexFormating
        {
            get { return (DecodeIndexFormatingElement)this["DecodeIndexFormating"]; }
            set { this["DecodeIndexFormating"] = value; }
        }
		
		[ConfigurationProperty("SearchIndexSettings", IsDefaultCollection = true)]
        public SearchIndexElementCollection SearchIndexSettings
        {
            get { return (SearchIndexElementCollection)this["SearchIndexSettings"]; }
            set { this["SearchIndexSettings"] = value; }
        }
    }
}