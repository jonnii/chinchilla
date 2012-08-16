using System;
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
        /// The queue that is being subscribed to by this subscription
        /// </summary>
        IQueue Queue { get; }

        /// <summary>
        /// Starts the subscription
        /// </summary>
        void Start();
    }
}