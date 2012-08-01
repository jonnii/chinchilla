using System.Collections.Generic;
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
            var settings = HttpClientSettings.Default;
            settings.Authenticator = new BasicAuthenticator("guest", "guest");
            settings.Configure<JsonDotNetSerializer>(s =>
                s.ConfigureSettings(c =>
                    c.ContractResolver = new RabbitContractResolver()));

            return HttpClient.Create(root, settings);
        }

        public IEnumerable<VirtualHost> VirtualHosts
        {
            get
            {
                return Client.Get("vhosts").OnOk().As<List<VirtualHost>>();
            }
        }

        public bool Create(VirtualHost virtualHost)
        {
            return Client.Put("vhosts/:name", new { name = virtualHost.Name }).Is(HttpStatusCode.NoContent);
        }

        public bool Delete(VirtualHost virtualHost)
        {
            return Client.Delete("vhosts/:name", new { name = virtualHost.Name }).Is(HttpStatusCode.NoContent);
        }

        public bool Create(VirtualHost virtualHost, User user, Permissions permissions)
        {
            return Client.Put(
                permissions,
                "permissions/:vhost/:user", new { vhost = virtualHost.Name, user = user.Name })
            .Is(HttpStatusCode.NoContent);
        }
    }
}