using System;
using RabbitMQ.Client;

namespace Chinchilla
{
    /// <summary>
    /// The connection factory is responsible for creating a rabbitmq connection
    /// </summary>
    public interface IConnectionFactory
    {
        /// <summary>
        /// Creates a connection for a URI
        /// </summary>
        IConnection Create(Uri uri);
    }
}