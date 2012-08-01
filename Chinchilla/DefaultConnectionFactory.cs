using System;
using RabbitMQ.Client;

namespace Chinchilla
{
    public class DefaultConnectionFactory : IConnectionFactory
    {
        private readonly ConnectionFactory connectionFactory;

        public DefaultConnectionFactory()
            : this(new ConnectionFactory())
        {

        }

        public DefaultConnectionFactory(ConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public IConnection Create(Uri uri)
        {
            connectionFactory.Uri = uri.ToString();
            return connectionFactory.CreateConnection();
        }
    }
}