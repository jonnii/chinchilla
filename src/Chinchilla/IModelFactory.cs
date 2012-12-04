using System;
using RabbitMQ.Client;

namespace Chinchilla
{
    /// <summary>
    /// The model factory is responsible for creating and tracking model references
    /// </summary>
    public interface IModelFactory : IDisposable
    {
        /// <summary>
        /// Creates a model reference
        /// </summary>
        IModelReference CreateModel();

        /// <summary>
        /// Creates a model reference with a tag
        /// </summary>
        IModelReference CreateModel(string tag);

        /// <summary>
        /// Reconnects all the model references tracked by this model factory with a new
        /// connection
        /// </summary>
        void Reconnect(IConnection newConnection);
    }
}