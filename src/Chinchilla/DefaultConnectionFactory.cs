using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Authentication;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using Chinchilla.Logging;

namespace Chinchilla
{
    public class DefaultConnectionFactory : IConnectionFactory
    {
        private readonly Uri[] uris;

        private readonly ILogger logger = Logger.Create<DefaultConnectionFactory>();

        private readonly Dictionary<string, IModelFactory> modelFactories =
            new Dictionary<string, IModelFactory>();

        public DefaultConnectionFactory(Uri[] uris)
        {
            this.uris = uris;

            MaxRetries = 100;
        }

        public int MaxRetries { get; set; }

        public SslOption SslOptions { get; set; }

        public IModelFactory Create()
        {
            var connectionFactory = CreateConnectionFactory(uris[0]);

            var connection = CreateConnection(connectionFactory);
            return CreateModelFactory(connection);
        }

        private ConnectionFactory CreateConnectionFactory(Uri uri)
        {
            logger.InfoFormat("Creating connnection factory Uri='{0}'", uri);

            var connectionFactory = new ConnectionFactory();

            connectionFactory.ClientProperties["MachineName"] = Environment.MachineName;
            connectionFactory.ClientProperties["User"] =
                string.Concat(Environment.UserDomainName, "\\", Environment.UserName);
            connectionFactory.ClientProperties["ConnectionFactory.CreatedAt"] =
                DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);

            connectionFactory.Uri = uri.ToString();

            // Set SSL options *after* the URI, as setting the URI removes the SSL settings
            if (SslOptions != null)
            {
                connectionFactory.Ssl = SslOptions;
            }

            return connectionFactory;
        }

        private IConnection CreateConnection(ConnectionFactory connectionFactory)
        {
            var newConnection = CreateConnectionWithRetries(connectionFactory);

            newConnection.ConnectionShutdown += ConnectionOnConnectionShutdown;

            return newConnection;
        }

        private IConnection CreateConnectionWithRetries(ConnectionFactory connectionFactory)
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
                    var connection = connectionFactory.CreateConnection();

                    connection.ClientProperties["ConnectionFactory.CreatedAt"] =
                        DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);

                    return connection;
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
            var connectionFactory = CreateConnectionFactory(uris[0]);

            var newConnection = CreateConnection(connectionFactory);
            var connectionEndPoint = connection.Endpoint.ToString();

            var modelFactory = modelFactories[connectionEndPoint];
            modelFactory.Reconnect(newConnection);
        }
    }
}