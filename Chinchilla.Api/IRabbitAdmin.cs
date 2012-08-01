using System.Collections.Generic;

namespace Chinchilla.Api
{
    public interface IRabbitAdmin
    {
        IEnumerable<VirtualHost> VirtualHosts { get; }

        bool Create(VirtualHost virtualHost);

        bool Delete(VirtualHost virtualHost);

        bool Create(VirtualHost virtualHost, User user, Permissions permissions);
    }
}
