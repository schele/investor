using Investor.ToggleLanguage.Abstraction;

namespace Investor.ToggleLanguage
{
    
public abstract class ToggleLanguageBase : IToggleLanguage
    {
        protected string Url { get; private set; }
        protected string Name { get; private set; }

        protected ToggleLanguageBase(string name, string url)
        {
            Name = name;
            Url = url;
        }

        public abstract string GetUrl();
        public abstract string GetLanguage();
    }
}