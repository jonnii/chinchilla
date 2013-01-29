using System.Linq;

namespace Chinchilla
{
    /// <summary>
    /// A subscription state is a breakdown of what the current subscription is doing
    /// </summary>
    public class SubscriptionState
    {
        public SubscriptionState(
            bool isStarted,
            bool isStartable,
            QueueState[] queueStates,
            WorkerState[] workerStates)
        {
            IsStarted = isStarted;
            IsStartable = isStartable;
            QueueStates = queueStates;
            Workers = workerStates;
        }

        /// <summary>
        /// Indicates whether or not this subscription has been started
        /// </summary>
        public bool IsStarted { get; set; }

        /// <summary>
        /// Indicates whether or not this subscription is startable
        /// </summary>
        public bool IsStartable { get; set; }

        /// <summary>
        /// The state of all the queues on this subscription
        /// </summary>
        public QueueState[] QueueStates { get; set; }

        /// <summary>
        /// The state of all the workers in this subscription
        /// </summary>
        public WorkerState[] Workers { get; set; }

        /// <summary>
        /// The total number of accepted messages
        /// </summary>
        public long TotalAcceptedMessages()
        {
            return QueueStates.Sum(q => q.NumAcceptedMessages);
        }

        /// <summary>
        /// The total number of failed messages
        /// </summary>
        public long TotalFailedMessages()
        {
            return QueueStates.Sum(q => q.NumFailedMessages);
        }
    }
}