using System;
using System.Threading;
using System.Threading.Tasks;
using Chinchilla.Api;
using Chinchilla.Logging;
using Xunit;

namespace Chinchilla.Integration
{
    public class Feature
    {
        protected void WaitFor(Func<bool> condition)
        {
            SpinWait.SpinUntil(() => condition(), TimeSpan.FromSeconds(5));
        }
    }

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