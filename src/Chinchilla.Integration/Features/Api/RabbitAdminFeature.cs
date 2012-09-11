using System.Linq;
using Chinchilla.Api;
using NUnit.Framework;

namespace Chinchilla.Integration.Features.Api
{
    [TestFixture]
    public class RabbitAdminFeature
    {
        [Test]
        public void ShouldCreateVirtualHost()
        {
            var api = new RabbitAdmin("http://localhost:55672/api");

            api.Delete(new VirtualHost("test"));

            Assert.That(api.Create(new VirtualHost("test")));
            Assert.That(api.VirtualHosts.Any(v => v.Name == "test"));
        }
    }
}
