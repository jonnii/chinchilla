using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Chinchilla.Api.Extensions;

namespace Chinchilla.Api
{
    public class RabbitAdmin : IRabbitAdmin
    {
        private readonly RabbitHttpClient httpClient;

        public RabbitAdmin(string root)
            : this(root, "guest", "guest")
        {
        }

        public RabbitAdmin(string root, string username, string password)
        {
            httpClient = new RabbitHttpClient(root.FormatWithReplacements(), username, password);
        }

        public Task<IEnumerable<VirtualHost>> VirtualHostsAsync()
        {
            return httpClient.ExecuteList<VirtualHost>("vhosts");
        }

        public Task<IEnumerable<Connection>> ConnectionsAsync()
        {
            return httpClient.ExecuteList<Connection>("connections");
        }

        public Task<IEnumerable<Exchange>> ExchangesAsync(VirtualHost virtualHost)
        {
            return httpClient.ExecuteList<Exchange>("exchanges/{vhost}", new Dictionary<string, string> { { "vhost", virtualHost.Name } });
        }

        public Task<IEnumerable<Queue>> QueuesAsync(VirtualHost virtualHost)
        {
            return httpClient.ExecuteList<Queue>("queues/{vhost}", new Dictionary<string, string> { { "vhost", virtualHost.Name } });
        }

        public Task<IEnumerable<Permissions>> PermissionsAsync(VirtualHost virtualHost)
        {
            return httpClient.ExecuteList<Permissions>("vhosts/{name}/permissions", new Dictionary<string, string> { { "name", virtualHost.Name } });
        }

        public async Task<bool> CreateAsync(VirtualHost virtualHost)
        {
            var response = await httpClient.Execute(HttpMethod.Put,
                "vhosts/{name}",
                new Dictionary<string, string>
                {
                    {"name", virtualHost.Name}
                });

            return response.StatusCode == HttpStatusCode.Created;
        }

        public Task<bool> CreateAsync(VirtualHost virtualHost, Queue queue)
        {
            return CreateAsync(virtualHost, queue, QueueOptions.Default);
        }

        public async Task<bool> CreateAsync(VirtualHost virtualHost, Queue queue, QueueOptions options)
        {
            var response = await httpClient.Execute(HttpMethod.Put,
                "queues/{vhost}/{name}",
                new Dictionary<string, string>
                {
                    {"vhost", virtualHost.Name},
                    {"name", queue.Name}
                },
                options);

            return response.StatusCode == HttpStatusCode.NoContent;
        }

        public Task<bool> CreateAsync(VirtualHost virtualHost, Exchange exchange)
        {
            return CreateAsync(virtualHost, exchange, ExchangeOptions.Default);
        }

        public async Task<bool> CreateAsync(VirtualHost virtualHost, Exchange exchange, ExchangeOptions options)
        {
            var response = await httpClient.Execute(HttpMethod.Put,
                "exchanges/{vhost}/{name}",
                new Dictionary<string, string>
                {
                    {"vhost", virtualHost.Name},
                    {"name", exchange.Name}
                },
                options);

            return response.StatusCode == HttpStatusCode.NoContent;
        }

        public Task<bool> CreateAsync(VirtualHost virtualHost, Exchange exchange, Queue queue)
        {
            return CreateAsync(virtualHost, exchange, queue, BindingOptions.Default);
        }

        public async Task<bool> CreateAsync(VirtualHost virtualHost, Exchange exchange, Queue queue, BindingOptions options)
        {
            var response = await httpClient.Execute(
                HttpMethod.Post,
                "bindings/{vhost}/e/{exchange}/q/{queue}",
                new Dictionary<string, string> { { "vhost", virtualHost.Name }, { "exchange", exchange.Name }, { "queue", queue.Name } },
                options);

            return response.StatusCode == HttpStatusCode.Created;
        }

        public async Task<bool> DeleteAsync(VirtualHost virtualHost)
        {
            var response = await httpClient.Execute(HttpMethod.Delete,
                "vhosts/{name}",
                new Dictionary<string, string>
                {
                    {"name", virtualHost.Name}
                });

            return response.StatusCode == HttpStatusCode.NoContent;
        }

        public async Task<bool> DeleteAsync(Connection connection)
        {
            var response = await httpClient.Execute(HttpMethod.Delete,
                "connections/{name}",
                new Dictionary<string, string>
                {
                    {"name", connection.Name}
                });

            return response.StatusCode == HttpStatusCode.NoContent;
        }

        public async Task<bool> CreateAsync(VirtualHost virtualHost, User user, Permission permission)
        {
            var response = await httpClient.Execute(
                HttpMethod.Put,
                "permissions/{vhost}/{user}",
                new Dictionary<string, string> { { "vhost", virtualHost.Name }, { "user", user.Name } },
                permission
            );

            return response.StatusCode == HttpStatusCode.NoContent;
        }

        public async Task<bool> ExistsAsync(VirtualHost virtualHost, Queue queue)
        {
            var queues = await QueuesAsync(virtualHost);
            return queues.Any(q => q.Name == queue.Name);
        }

        public async Task<bool> ExistsAsync(VirtualHost virtualHost, Exchange exchange)
        {
            var exchanges = await ExchangesAsync(virtualHost);

            return exchanges.Any(e => e.Name == exchange.Name);
        }

        public Task<IEnumerable<Message>> MessagesAsync(VirtualHost virtualHost, Queue queue)
        {
            var options = new
            {
                vhost = virtualHost.Name,
                name = queue.Name,
                count = "1",
                requeue = "true",
                encoding = "auto",
                truncate = "50000"
            };

            return httpClient.ExecuteList<Message>(
                HttpMethod.Post,
                "queues/{vhost}/{queue}/get",
                new Dictionary<string, string> { { "vhost", virtualHost.Name }, { "queue", queue.Name } },
                options
            );
        }

        public IEnumerable<VirtualHost> VirtualHosts()
        {
            return VirtualHostsAsync().Result;
        }

        public IEnumerable<Connection> Connections()
        {
            return ConnectionsAsync().Result;
        }

        public IEnumerable<Exchange> Exchanges(VirtualHost virtualHost)
        {
            return ExchangesAsync(virtualHost).Result;
        }

        public IEnumerable<Queue> Queues(VirtualHost virtualHost)
        {
            return QueuesAsync(virtualHost).Result;
        }

        public IEnumerable<Permissions> Permissions(VirtualHost virtualHost)
        {
            return PermissionsAsync(virtualHost).Result;
        }

        public bool Create(VirtualHost virtualHost)
        {
            return CreateAsync(virtualHost).Result;
        }

        public bool Create(VirtualHost virtualHost, Queue queue)
        {
            return CreateAsync(virtualHost, queue).Result;
        }

        public bool Create(VirtualHost virtualHost, Queue queue, QueueOptions options)
        {
            return CreateAsync(virtualHost, queue, options).Result;
        }

        public bool Create(VirtualHost virtualHost, Exchange exchange)
        {
            return CreateAsync(virtualHost, exchange).Result;
        }

        public bool Create(VirtualHost virtualHost, Exchange exchange, ExchangeOptions options)
        {
            return CreateAsync(virtualHost, exchange, options).Result;
        }

        public bool Create(VirtualHost virtualHost, User user, Permission permission)
        {
            return CreateAsync(virtualHost, user, permission).Result;
        }

        public bool Create(VirtualHost virtualHost, Exchange exchange, Queue queue)
        {
            return CreateAsync(virtualHost, exchange, queue).Result;
        }

        public bool Create(VirtualHost virtualHost, Exchange exchange, Queue queue, BindingOptions options)
        {
            return CreateAsync(virtualHost, exchange, queue, options).Result;
        }

        public bool Exists(VirtualHost virtualHost, Queue queue)
        {
            return ExistsAsync(virtualHost, queue).Result;
        }

        public bool Exists(VirtualHost virtualHost, Exchange exchange)
        {
            return ExistsAsync(virtualHost, exchange).Result;
        }

        public bool Delete(VirtualHost virtualHost)
        {
            return DeleteAsync(virtualHost).Result;
        }

        public bool Delete(Connection connection)
        {
            return DeleteAsync(connection).Result;
        }

        public IEnumerable<Message> Messages(VirtualHost virtualHost, Queue queue)
        {
            return MessagesAsync(virtualHost, queue).Result;
        }
    }
}