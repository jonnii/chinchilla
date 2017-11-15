using System;
using System.Threading.Tasks;
using Chinchilla.Api;
using Chinchilla.Logging;

namespace Chinchilla.Integration
{
    public abstract class Feature : IDisposable
    {
        private IRabbitAdmin admin;

        public IRabbitAdmin Admin => admin ?? (admin = new RabbitAdmin("http://localhost:15672/api"));

        public VirtualHost VirtualHost => new VirtualHost(GetType().Name);

        protected async Task<string> CreateVHost()
        {
            Logger.Factory = new ConsoleLoggerFactory();

            var hostName = GetType().Name;

            var host = VirtualHost;

            await Admin.DeleteAsync(host).ConfigureAwait(false);
            await Admin.CreateAsync(host).ConfigureAwait(false);
            await Admin.CreateAsync(host, new User("guest"), Permission.All).ConfigureAwait(false);

            return hostName;
        }

        protected async Task<IBus> CreateBus(DepotSettings settings)
        {
            var vhost = await CreateVHost().ConfigureAwait(false);
            return Depot.Connect($"localhost/{vhost}", settings);
        }

        protected async Task<IBus> CreateBus()
        {
            var vhost = await CreateVHost().ConfigureAwait(false);
            return Depot.Connect($"localhost/{vhost}");
        }

        public void Dispose()
        {
            Admin.DeleteAsync(VirtualHost).Wait();
        }

        protected Task WaitFor(Func<bool> condition)
        {
            return Task.WhenAny(WaitForInner(condition), Task.Delay(TimeSpan.FromSeconds(5)));
        }

        private async Task WaitForInner(Func<bool> condition)
        {
            while (!condition())
            {
                await Task.Delay(10).ConfigureAwait(false);
            }
        }
    }
}
