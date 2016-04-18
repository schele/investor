using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investor.ToggleLanguage.Abstraction
{
    public interface IToggleLanguage
    {
        string GetUrl();
        string GetLanguage();
    }
}
