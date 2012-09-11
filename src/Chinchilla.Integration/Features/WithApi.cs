using Chinchilla.Api;
using NUnit.Framework;

namespace Chinchilla.Integration.Features
{
    public class WithApi : WithLogging
    {
        public readonly VirtualHost IntegrationVHost = new VirtualHost("integration");

        protected IRabbitAdmin admin;

        [SetUp]
        public void ResetVirtualHost()
        {
            admin = RabbitAdmin.Create("http://localhost:55672/api");
            admin.Delete(IntegrationVHost);
            admin.Create(IntegrationVHost);
            admin.Create(IntegrationVHost, new User("guest"), Permission.All);
        }
    }
}