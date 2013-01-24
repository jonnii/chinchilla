using System.Collections.Generic;

namespace Chinchilla.Api
{
    public interface IRabbitAdmin
    {
        IEnumerable<VirtualHost> VirtualHosts { get; }

        IEnumerable<Connection> Connections();

        IEnumerable<Exchange> Exchanges(VirtualHost virtualHost);

        IEnumerable<Queue> Queues(VirtualHost virtualHost);

        IEnumerable<Permissions> Permissions(VirtualHost virtualHost);

        bool Create(VirtualHost virtualHost);

        bool Create(VirtualHost virtualHost, Queue queue);

        bool Create(VirtualHost virtualHost, Queue queue, QueueOptions options);

        bool Create(VirtualHost virtualHost, Exchange exchange);

        bool Create(VirtualHost virtualHost, Exchange exchange, ExchangeOptions options);

        bool Create(VirtualHost virtualHost, User user, Permission permission);

        bool Create(VirtualHost virtualHost, Exchange exchange, Queue queue);

        bool Create(VirtualHost virtualHost, Exchange exchange, Queue queue, BindingOptions options);

        bool Exists(VirtualHost virtualHost, Queue queue);

        bool Exists(VirtualHost virtualHost, Exchange exchange);

        bool Delete(VirtualHost virtualHost);

        bool Delete(Connection connection);

        IEnumerable<Message> Messages(VirtualHost virtualHost, Queue queue);
    }
}
