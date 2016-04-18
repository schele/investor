using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Investor.ToggleLanguage
{
    public class ToggleLanguageObject : ToggleLanguageBase
    {
        public ToggleLanguageObject(string name, string url) : base(name, url) { }

        public override string GetUrl()
        {
            return Url;
        }

        public override string GetLanguage()
        {
            return Name;
        }
    }
}