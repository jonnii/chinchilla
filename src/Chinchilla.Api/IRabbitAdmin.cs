using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chinchilla.Api
{
    public interface IRabbitAdmin
    {
        Task<IEnumerable<VirtualHost>> VirtualHostsAsync();

        Task<IEnumerable<Connection>> ConnectionsAsync();

        Task<IEnumerable<Exchange>> ExchangesAsync(VirtualHost virtualHost);

        Task<IEnumerable<Queue>> QueuesAsync(VirtualHost virtualHost);

        Task<IEnumerable<Permissions>> PermissionsAsync(VirtualHost virtualHost);

        Task<bool> CreateAsync(VirtualHost virtualHost);

        Task<bool> CreateAsync(VirtualHost virtualHost, Queue queue);

        Task<bool> CreateAsync(VirtualHost virtualHost, Queue queue, QueueOptions options);

        Task<bool> CreateAsync(VirtualHost virtualHost, Exchange exchange);

        Task<bool> CreateAsync(VirtualHost virtualHost, Exchange exchange, ExchangeOptions options);

        Task<bool> CreateAsync(VirtualHost virtualHost, User user, Permission permission);

        Task<bool> CreateAsync(VirtualHost virtualHost, Exchange exchange, Queue queue);

        Task<bool> CreateAsync(VirtualHost virtualHost, Exchange exchange, Queue queue, BindingOptions options);

        Task<bool> ExistsAsync(VirtualHost virtualHost, Queue queue);

        Task<bool> ExistsAsync(VirtualHost virtualHost, Exchange exchange);

        Task<bool> DeleteAsync(VirtualHost virtualHost);

        Task<bool> DeleteAsync(Connection connection);

        Task<IEnumerable<Message>> MessagesAsync(VirtualHost virtualHost, Queue queue);



        IEnumerable<VirtualHost> VirtualHosts();

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
