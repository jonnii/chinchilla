using System.Threading.Tasks;

namespace Chinchilla.Api
{
    public interface IRabbitAdmin
    {
        Task<VirtualHost[]> VirtualHostsAsync();

        Task<Connection[]> ConnectionsAsync();

        Task<Exchange[]> ExchangesAsync(VirtualHost virtualHost);

        Task<Queue[]> QueuesAsync(VirtualHost virtualHost);

        Task<Permissions[]> PermissionsAsync(VirtualHost virtualHost);

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

        Task<Message[]> MessagesAsync(VirtualHost virtualHost, Queue queue);
    }
}
