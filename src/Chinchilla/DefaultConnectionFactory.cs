using System;
using System.Collections.Generic;
using System.Security.Authentication;
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

        public SslOption SslOptions { get; set; }

        public DefaultConnectionFactory()
            : this(new ConnectionFactory())
        {

        }

        public DefaultConnectionFactory(ConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;

            MaxRetries = 100;
        }

        public int MaxRetries { get; set; }

        public IModelFactory Create(Uri uri)
        {
            logger.InfoFormat("Creating connnection for {0}", uri);

            connectionFactory.Uri = uri.ToString();

            // Set SSL options *after* the URI, as setting the URI removes the SSL settings
            if (this.SslOptions != null)
            {
                connectionFactory.Ssl = this.SslOptions;
            }

            var connection = CreateConnection();
            return CreateModelFactory(connection);
        }

        private IConnection CreateConnection()
        {
            var newConnection = CreateConnectionWithRetries();

            newConnection.ConnectionShutdown += ConnectionOnConnectionShutdown;

            return newConnection;
        }

        private IConnection CreateConnectionWithRetries()
        {
            var numTries = 0;

            while (numTries < MaxRetries)
            {
                // Increase stand-off time after each retry, but never wait longer than 5 seconds
                var standoffTime = Math.Min(5000, numTries * 100);

                Thread.Sleep(standoffTime);

                logger.DebugFormat(" -> Attempting reconnect");

                try
                {
                    return connectionFactory.CreateConnection();
                }
                catch (BrokerUnreachableException ex)
                {
                    if (ex.InnerException is AuthenticationException)
                    {
                        logger.Error(ex.InnerException);
                        throw;
                    }

                    logger.DebugFormat("Reconnect attempt {0} failed.", numTries);
                    ++numTries;
                }
            }

            var message = string.Format(
                "Could not create connection, the number of retries ({0}) was exceeded", MaxRetries);

            throw new ChinchillaException(message);
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
            var newConnection = CreateConnection();
            var connectionEndPoint = connection.Endpoint.ToString();

            var modelFactory = modelFactories[connectionEndPoint];
            modelFactory.Reconnect(newConnection);
        }
    }
}