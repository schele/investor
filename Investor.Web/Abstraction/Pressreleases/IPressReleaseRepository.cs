using System.Collections.Generic;
using Investor.Models.Models;

namespace Investor.Abstraction.Pressreleases
{
    public interface IPressReleaseRepository
    {
        List<PressRelease> GetPressReleases();
    }
}