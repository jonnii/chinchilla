using Chinchilla.Logging;
using NUnit.Framework;

namespace Chinchilla.Integration
{
    public class WithLogging
    {
        [SetUp]
        public void Setup()
        {
            Logger.Factory = new ConsoleLoggerFactory();
        }
    }
}