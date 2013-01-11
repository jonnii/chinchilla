using RabbitMQ.Client.Events;

namespace Chinchilla
{
    public interface IDeliveryQueue : IDeliveryListener
    {
        /// <summary>
        /// The name of this delivery queue
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The number of messages accepted by this subscription
        /// </summary>
        long NumAcceptedMessages { get; }

        /// <summary>
        /// The number of failed messages processed by this subscription
        /// </summary>
        long NumFailedMessages { get; }

        /// <summary>
        /// Tries to take a message from the delivery queue
        /// </summary>
        bool TryTake(out BasicDeliverEventArgs item);

        /// <summary>
        /// Starts the delivery queue
        /// </summary>
        void Start();

        /// <summary>
        /// Gets the state of this queue
        /// </summary>
        QueueState GetState();
    }
}