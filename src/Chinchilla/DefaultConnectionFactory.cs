using System;
using System.Collections.Generic;
using Chinchilla.Logging;
using RabbitMQ.Client;

namespace Chinchilla
{
    public class DefaultConnectionFactory : IConnectionFactory
    {
        private readonly ILogger logger = Logger.Create<DefaultConnectionFactory>();

        private readonly ConnectionFactory connectionFactory;

        private readonly Dictionary<string, IModelFactory> modelFactories =
            new Dictionary<string, IModelFactory>();

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

            var connection = CreateConnection();
            return CreateModelFactory(connection);
        }

        private IConnection CreateConnection()
        {
            var connection = connectionFactory.CreateConnection();
            connection.ConnectionShutdown += ConnectionOnConnectionShutdown;
            return connection;
        }

        private IModelFactory CreateModelFactory(IConnection connection)
        {
            var modelFactory = new ModelFactory(connection);

            modelFactories.Add(connection.Endpoint.ToString(), modelFactory);

            return modelFactory;
        }

        private void ConnectionOnConnectionShutdown(
            IConnection connection,
            ShutdownEventArgs reason)
        {
            logger.DebugFormat("Connection closed: {0}", connection.Endpoint.ToString());

            connection.ConnectionShutdown -= ConnectionOnConnectionShutdown;

            if (reason.Initiator == ShutdownInitiator.Application)
            {
                return;
            }

            TryReconnect(connection);
        }

        private void TryReconnect(IConnection connection)
        {
            logger.DebugFormat(" -> Attempting reconnect");

            var newConnection = CreateConnection();

            var connectionEndPoint = connection.Endpoint.ToString();
            var modelFactory = modelFactories[connectionEndPoint];
            modelFactory.Reconnect(newConnection);
        }
    }
}