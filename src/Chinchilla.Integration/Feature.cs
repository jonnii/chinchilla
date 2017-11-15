using System;
using System.Threading;

namespace Chinchilla.Integration
{
    public class Feature
    {
        protected void WaitFor(Func<bool> condition)
        {
            SpinWait.SpinUntil(condition, TimeSpan.FromSeconds(5));
        }
    }
}
