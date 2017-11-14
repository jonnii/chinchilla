using System.Linq;
using System.Threading.Tasks;
using Chinchilla.Api;
using Xunit;

namespace Chinchilla.Integration.Features.Api
{
    public class RabbitAdminFeature
    {
        [Fact]
        public async Task ShouldCreateVirtualHost()
        {
            var api = new RabbitAdmin("http://localhost:15672/api");

            await api.DeleteAsync(new VirtualHost("test"));

            Assert.True(api.Create(new VirtualHost("test")));
            Assert.True(api.VirtualHosts().Any(v => v.Name == "test"));
        }
    }
}
