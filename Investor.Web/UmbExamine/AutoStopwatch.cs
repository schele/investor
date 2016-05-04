using System;
using System.Diagnostics;

namespace Investor.UmbExamine
{
    public class AutoStopwatch : Stopwatch, IDisposable
    {
        public AutoStopwatch()
        {
            Start();
        }

        public void Dispose()
        {
           Stop();
        }
    }
}