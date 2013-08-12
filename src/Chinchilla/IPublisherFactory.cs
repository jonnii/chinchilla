using System;
using Chinchilla.Configuration;

namespace Chinchilla
{
    /// <summary>
    /// The publisher factory is responsible for creating and managing publishers
    /// </summary>
    public interface IPublisherFactory : IDisposable
    {
        /// <summary>
        /// Creates a publisher
        /// </summary>
        /// <typeparam name="TMessage">The type of message that this publisher can publish</typeparam>
        /// <param name="modelReference">The model reference this publisher will use</param>
        /// <param name="configuration">The configuration for this publisher</param>
        /// <returns>A publisher</returns>
        IPublisher<TMessage> Create<TMessage>(
            IModelReference modelReference,
            IPublisherConfiguration<TMessage> configuration);
    }
}
