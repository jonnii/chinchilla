using System;
using RabbitMQ.Client;

namespace Chinchilla
{
    public class ConnectionFactoryAdapter : IConnectionFactory
    {
        private readonly ConnectionFactory connectionFactory;

        public ConnectionFactoryAdapter()
            : this(new ConnectionFactory())
        {

        }

        public ConnectionFactoryAdapter(ConnectionFactory connectionFactory)
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