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

            Task.Run(async () =>
            {
                await Admin.DeleteAsync(IntegrationVHost);
                await Admin.CreateAsync(IntegrationVHost);
                await Admin.CreateAsync(IntegrationVHost, new User("guest"), Permission.All);
            });
        }

        public IRabbitAdmin Admin { get; }

        public void Dispose()
        {
            Task.Run(async () => await Admin.DeleteAsync(IntegrationVHost));
        }
    }
}