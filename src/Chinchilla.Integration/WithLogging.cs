using System;
using Chinchilla.Api;
using Chinchilla.Logging;
using Xunit;

namespace Chinchilla.Integration
{
    [CollectionDefinition("Rabbit Collection")]
    public class RabbitCollection : ICollectionFixture<RabbitFixture>
    {
    }

    public class RabbitFixture : IDisposable
    {
        public readonly VirtualHost IntegrationVHost = new VirtualHost("integration");

        public RabbitFixture()
        {
            Logger.Factory = new ConsoleLoggerFactory();

            Admin = new RabbitAdmin("http://localhost:15672/api");

            Admin.DeleteAsync(IntegrationVHost).Wait();
            Admin.CreateAsync(IntegrationVHost).Wait();
            Admin.CreateAsync(IntegrationVHost, new User("guest"), Permission.All).Wait();
        }

        public IRabbitAdmin Admin { get; }

        public void Dispose()
        {
            Admin.DeleteAsync(IntegrationVHost).Wait();
        }
    }
}