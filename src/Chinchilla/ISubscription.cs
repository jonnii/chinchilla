using System;

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
        long NumAcceptedMessages { get; }

        /// <summary>
        /// The number of failed messages processed by this subscription
        /// </summary>
        long NumFailedMessages { get; }

        /// <summary>
        /// The queues that are being subscribed to by this subscription
        /// </summary>
        IDeliveryQueue[] Queues { get; }

        /// <summary>
        /// Indicates whether or not this subscription can be started
        /// </summary>
        bool IsStartable { get; }

        /// <summary>
        /// Indicates whether or not this subscription has been started
        /// </summary>
        bool IsStarted { get; }

        /// <summary>
        /// Starts the subscription
        /// </summary>
        void Start();
    }
}