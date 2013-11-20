using System.Collections.Generic;
using System.Linq;
using System.Net;
using Chinchilla.Api.Extensions;

namespace Chinchilla.Api
{
    public class RabbitAdmin : IRabbitAdmin
    {
        private readonly HttpClient httpClient;

        public RabbitAdmin(string root)
        {
            httpClient = new HttpClient(root.FormatWithReplacements());
        }

        public IEnumerable<VirtualHost> VirtualHosts
        {
            get
            {
                return httpClient.Execute<List<VirtualHost>>("vhosts");
            }
        }

        public IEnumerable<Connection> Connections()
        {
            return httpClient.Execute<List<Connection>>("connections");
        }

        public IEnumerable<Exchange> Exchanges(VirtualHost virtualHost)
        {
            return httpClient.Execute<List<Exchange>>("exchanges/{vhost}", new Dictionary<string, string> { { "vhost", virtualHost.Name }});
        }

        public IEnumerable<Queue> Queues(VirtualHost virtualHost)
        {
            return httpClient.Execute<List<Queue>>("queues/{vhost}", new Dictionary<string, string> { { "vhost", virtualHost.Name } });
        }

        public IEnumerable<Permissions> Permissions(VirtualHost virtualHost)
        {
            return httpClient.Execute<List<Permissions>>("vhosts/{name}/permissions", new Dictionary<string, string> { {"name", virtualHost.Name} });
        }

        public bool Create(VirtualHost virtualHost)
        {
            return httpClient.Execute("vhosts/{name}", new Dictionary<string, string> { {"name", virtualHost.Name} }, "PUT")
                    .StatusCode == HttpStatusCode.NoContent;
        }

        public bool Create(VirtualHost virtualHost, Queue queue)
        {
            return Create(virtualHost, queue, QueueOptions.Default);
        }

        public bool Create(VirtualHost virtualHost, Queue queue, QueueOptions options)
        {
            return httpClient.Execute(
                "queues/{vhost}/{name}", 
                new Dictionary<string, string> { { "vhost", virtualHost.Name }, { "name", queue.Name } }, 
                "PUT", 
                options
            ).StatusCode == HttpStatusCode.NoContent;
        }

        public bool Create(VirtualHost virtualHost, Exchange exchange)
        {
            return Create(virtualHost, exchange, ExchangeOptions.Default);
        }

        public bool Create(VirtualHost virtualHost, Exchange exchange, ExchangeOptions options)
        {
            return httpClient.Execute(
                "exchanges/{vhost}/{name}", 
                new Dictionary<string, string> { { "vhost", virtualHost.Name }, { "name", exchange.Name} },
                "PUT",
                options
            ).StatusCode == HttpStatusCode.NoContent;
        }

        public bool Create(VirtualHost virtualHost, Exchange exchange, Queue queue)
        {
            return Create(virtualHost, exchange, queue, BindingOptions.Default);
        }

        public bool Create(VirtualHost virtualHost, Exchange exchange, Queue queue, BindingOptions options)
        {
            return httpClient.Execute(
                "bindings/{vhost}/e/{exchange}/q/{queue}",
                new Dictionary<string, string> { { "vhost", virtualHost.Name }, { "exchange", exchange.Name }, { "queue", queue.Name } },
                "POST",
                options
            ).StatusCode == HttpStatusCode.Created;
        }

        public bool Delete(VirtualHost virtualHost)
        {
            return httpClient.Execute("vhosts/{name}", new Dictionary<string, string>{ { "name", virtualHost.Name } }, "DELETE").
                StatusCode == HttpStatusCode.NoContent;
        }

        public bool Delete(Connection connection)
        {
            return httpClient.Execute("connections/{name}", new Dictionary<string, string>{ { "name", connection.Name } }, "DELETE").
                StatusCode == HttpStatusCode.NoContent;
        }

        public bool Create(VirtualHost virtualHost, User user, Permission permission)
        {
            return httpClient.Execute(
                "permissions/{vhost}/{user}",
                new Dictionary<string, string> { { "vhost", virtualHost.Name }, { "user", user.Name } },
                "PUT", 
                permission
            ).StatusCode == HttpStatusCode.NoContent;
        }

        public bool Exists(VirtualHost virtualHost, Queue queue)
        {
            return Queues(virtualHost).Any(q => q.Name == queue.Name);
        }

        public bool Exists(VirtualHost virtualHost, Exchange exchange)
        {
            return Exchanges(virtualHost).Any(e => e.Name == exchange.Name);
        }

        public IEnumerable<Message> Messages(VirtualHost virtualHost, Queue queue)
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

            return httpClient.Execute<List<Message>>(
                "queues/{vhost}/{queue}/get",
                new Dictionary<string, string> { { "vhost", virtualHost.Name }, { "queue", queue.Name } },
                "POST",
                options
            );
        }
    }
}