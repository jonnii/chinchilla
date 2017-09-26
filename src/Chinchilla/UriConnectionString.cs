using System;
using RabbitMQ.Client;

namespace Chinchilla
{
    public class UriConnectionString : ConnectionString
    {
        private readonly string uri;

        public UriConnectionString(string uri)
        {
            this.uri = uri;
        }

        public override void Apply(ConnectionFactory connection)
        {
            connection.Uri = new Uri(uri);
        }
    }
}