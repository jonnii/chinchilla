using System;
using Chinchilla.Logging;
using RabbitMQ.Client;

namespace Chinchilla
{
    public class DefaultConnectionFactory : IConnectionFactory
    {
        private readonly ILogger logger = Logger.Create<DefaultConnectionFactory>();

        private readonly ConnectionFactory connectionFactory;

        public DefaultConnectionFactory()
            : this(new ConnectionFactory())
        {

        }

        public DefaultConnectionFactory(ConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public IModelFactory Create(Uri uri)
        {
            logger.InfoFormat("Creating connnection for {0}", uri);

            connectionFactory.Uri = uri.ToString();

            var connection = connectionFactory.CreateConnection();
            return new ModelFactory(connection);
        }
    }
}