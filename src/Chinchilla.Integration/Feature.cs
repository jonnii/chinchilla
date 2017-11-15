using System;
using System.Threading;
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

            await Admin.DeleteAsync(host);
            await Admin.CreateAsync(host);
            await Admin.CreateAsync(host, new User("guest"), Permission.All);

            return hostName;
        }

        protected async Task<IBus> CreateBus()
        {
            var vhost = await CreateVHost();
            return Depot.Connect($"localhost/{vhost}");
        }

        public void Dispose()
        {
            Admin.DeleteAsync(VirtualHost).Wait();
        }

        protected void WaitFor(Func<bool> condition)
        {
            SpinWait.SpinUntil(condition, TimeSpan.FromSeconds(5));
        }
    }
}
