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

        protected async Task<string> CreateVHost()
        {
            Logger.Factory = new ConsoleLoggerFactory();

            var hostName = GetType().Name;

            var host = new VirtualHost(GetType().Name);

            await Admin.DeleteAsync(host);
            await Admin.CreateAsync(host);
            await Admin.CreateAsync(host, new User("guest"), Permission.All);

            return hostName;
        }

        public IRabbitAdmin Admin => admin ?? (admin = new RabbitAdmin("http://localhost:15672/api"));

        public void Dispose()
        {
            var host = new VirtualHost(GetType().Name);
            Admin.DeleteAsync(host).Wait();
        }

        protected void WaitFor(Func<bool> condition)
        {
            SpinWait.SpinUntil(condition, TimeSpan.FromSeconds(5));
        }
    }
}
