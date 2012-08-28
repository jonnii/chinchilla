using System;

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
        IModelFactory Create(Uri uri);
    }
}