using System.Collections.Generic;
using System.Linq;
using System.Net;
using SpeakEasy;
using SpeakEasy.Authenticators;
using SpeakEasy.Serializers;

namespace Chinchilla.Api
{
    public class RabbitAdmin : IRabbitAdmin
    {
        public static IRabbitAdmin Create(string root)
        {
            return new RabbitAdmin(root);
        }

        private readonly string root;

        private IHttpClient httpClient;

        private RabbitAdmin(string root)
        {
            this.root = root;
        }

        public IHttpClient Client
        {
            get
            {
                return httpClient ?? (httpClient = BuildClient());
            }
        }

        private IHttpClient BuildClient()
        {
            var settings = new HttpClientSettings
            {
                Authenticator = new BasicAuthenticator("guest", "guest")
            };

            settings.Configure<JsonDotNetSerializer>(s =>
                s.ConfigureSettings(c =>
                {
                    c.ContractResolver = new RabbitContractResolver();
                }));

            return HttpClient.Create(root, settings);
        }

        public IEnumerable<VirtualHost> VirtualHosts
        {
            get
            {
                return Client.Get("vhosts").OnOk().As<List<VirtualHost>>();
            }
        }

        public IEnumerable<Connection> Connections()
        {
            return Client.Get("connections").OnOk().As<List<Connection>>();
        }

        public IEnumerable<Exchange> Exchanges(VirtualHost virtualHost)
        {
            return Client.Get("exchanges/:vhost", new { vhost = virtualHost.Name }).OnOk().As<List<Exchange>>();
        }

        public IEnumerable<Queue> Queues(VirtualHost virtualHost)
        {
            return Client.Get("queues/:vhost", new { vhost = virtualHost.Name }).OnOk().As<List<Queue>>();
        }

        public bool Create(VirtualHost virtualHost)
        {
            return Client.Put("vhosts/:name", new { name = virtualHost.Name }).Is(HttpStatusCode.NoContent);
        }

        public bool Delete(VirtualHost virtualHost)
        {
            return Client.Delete("vhosts/:name", new { name = virtualHost.Name }).Is(HttpStatusCode.NoContent);
        }

        public bool Delete(Connection connection)
        {
            return Client.Delete("connections/:name", new { name = connection.Name }).Is(HttpStatusCode.NoContent);
        }

        public bool Create(VirtualHost virtualHost, User user, Permissions permissions)
        {
            return Client.Put(
                permissions,
                "permissions/:vhost/:user", new { vhost = virtualHost.Name, user = user.Name })
            .Is(HttpStatusCode.NoContent);
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

            var resource = Client.BuildRelativeResource("queues/:vhost/:queue/get", new
            {
                vhost = virtualHost.Name,
                queue = queue.Name
            });

            var postRequest = new PostRequest(resource, new ObjectRequestBody(options));

            // need to set accept to empty string or the http management plugin
            // will complain
            postRequest.AddHeader("Accept", string.Empty);

            return Client.Run(postRequest)
                .OnOk()
                .As<List<Message>>();
        }
    }
}