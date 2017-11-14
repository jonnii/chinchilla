using System;
using Chinchilla.Logging;
using Xunit;

namespace Chinchilla.Integration
{
    // public class WithLogging
    // {
    //     [SetUp]
    //     public void Setup()
    //     {
    //         Logger.Factory = new ConsoleLoggerFactory();
    //     }
    // }

    [CollectionDefinition("Api collection")]
    public class ApiCollection : ICollectionFixture<ApiFixture>
    {
    }

    public class ApiFixture : IDisposable
    {
        public void Dispose()
        {
            
        }
    }
}