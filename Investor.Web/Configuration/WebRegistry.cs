using Investor.Abstraction.Pressreleases;
using StructureMap.Configuration.DSL;

namespace Investor.Configuration
{
    public class WebRegistry : Registry
    {
        public WebRegistry()
        {
            For<IPressReleaseRepository>()
                .Use<PressReleaseRepository>();            
        }    
    }
}