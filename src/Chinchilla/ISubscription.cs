using System;
using System.Collections.Generic;
using Chinchilla.Topologies.Model;

namespace Chinchilla
{
    /// <summary>
    /// A subscription is a handle to a subscribed action
    /// </summary>
    public interface ISubscription : IDisposable
    {
        /// <summary>
        /// The number of messages accepted by this subscription
        /// </summary>
        ulong NumAcceptedMessages { get; }

        /// <summary>
        /// The number of failed messages processed by this subscription
        /// </summary>
        ulong NumFailedMessages { get; }

        /// <summary>
        /// The queues that are being subscribed to by this subscription
        /// </summary>
        IEnumerable<IQueue> Queues { get; }

        /// <summary>
        /// Starts the subscription
        /// </summary>
        void Start();
    }
}