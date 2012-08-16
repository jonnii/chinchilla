using System;
using Chinchilla.Topologies.Model;

namespace Chinchilla
{
    /// <summary>
    /// A publish channel is used to publish messages to an exchange
    /// </summary>
    public interface IPublisher<in TMessage> : IDisposable
    {
        /// <summary>
        /// The number of published messages
        /// </summary>
        long NumPublishedMessages { get; }

        /// <summary>
        /// The exchange that is published to
        /// </summary>
        IExchange Exchange { get; }

        /// <summary>
        /// Publishes a message
        /// </summary>
        /// <typeparam name="TMessage">The type of message to publish</typeparam>
        void Publish(TMessage message);
    }
}