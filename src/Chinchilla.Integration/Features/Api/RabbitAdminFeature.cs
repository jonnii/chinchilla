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

            await api.DeleteAsync(new VirtualHost("test")).ConfigureAwait(false);

            var created = await api.CreateAsync(new VirtualHost("test")).ConfigureAwait(false);
            Assert.True(created);

            var hosts = await api.VirtualHostsAsync().ConfigureAwait(false);
            Assert.Contains("test", hosts.Select(v => v.Name));
        }
    }
}
