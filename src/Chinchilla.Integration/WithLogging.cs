using System;
using Chinchilla.Api;
using Chinchilla.Logging;
using Xunit;

namespace Chinchilla.Integration
{
    [CollectionDefinition("Api collection")]
    public class ApiCollection : ICollectionFixture<ApiFixture>
    {
    }

    public class ApiFixture : IDisposable
    {
        public readonly VirtualHost IntegrationVHost = new VirtualHost("integration");

        public ApiFixture()
        {
            Logger.Factory = new ConsoleLoggerFactory();

            Admin = new RabbitAdmin("http://localhost:15672/api");
            Admin.Delete(IntegrationVHost);
            Admin.Create(IntegrationVHost);
            Admin.Create(IntegrationVHost, new User("guest"), Permission.All);
        }

        public IRabbitAdmin Admin { get; }

        public void Dispose()
        {  
        }
    }
}