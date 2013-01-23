using System;
using Chinchilla.Topologies.Model;

namespace Chinchilla
{
    /// <summary>
    /// A publisher is used to publish messages to an exchange
    /// </summary>
    public interface IPublisher<in TMessage> : IDisposable
    {
        /// <summary>
        /// The model reference attached to this publisher
        /// </summary>
        IModelReference ModelReference { get; }

        /// <summary>
        /// The queue or exchange that is being published to
        /// </summary>
        IBindable PublishTarget { get; }

        /// <summary>
        /// The number of published messages
        /// </summary>
        long NumPublishedMessages { get; }

        /// <summary>
        /// Publishes a message
        /// </summary>
        /// <typeparam name="TMessage">The type of message to publish</typeparam>
        void Publish(TMessage message);
    }
}