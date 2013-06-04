using System;
using System.Collections.Generic;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using Chinchilla.Logging;

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
            int numTries = 0;

            while (!connection.IsOpen)
            {
                // Increase stand-off time after each retry, but never wait longer than 5 seconds
                int standoffTime = Math.Min(5000, numTries * 100);
                Thread.Sleep(standoffTime);

                logger.DebugFormat(" -> Attempting reconnect");

                IConnection newConnection;

                try
                {
                    newConnection = CreateConnection();
                }
                catch (BrokerUnreachableException ex)
                {
                    logger.DebugFormat("Reconnect attempt {0} failed: {1}", numTries, connection.Endpoint.ToString());
                    numTries++;
                    continue;
                }

                var connectionEndPoint = connection.Endpoint.ToString();
                var modelFactory = modelFactories[connectionEndPoint];
                modelFactory.Reconnect(newConnection);
            }
        }
    }
}