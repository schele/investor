using System.Collections.Generic;
using System.Xml.Linq;
using com.sun.org.apache.xerces.@internal.xni;
using Investor.Models.Models;
using umbraco.presentation.translation;

namespace Investor.Abstraction.Pressreleases
{
    public interface IPressReleaseRepository
    {
        XDocument Load(string xml);

        List<PressRelease> GetPressReleases();

        PressRelease GetPressRelease(string id);
    }
}