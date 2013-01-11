using System.Linq;

namespace Chinchilla
{
    /// <summary>
    /// A subscription state is a breakdown of what the current subscription is doing
    /// </summary>
    public class SubscriptionState
    {
        public SubscriptionState(
            QueueState[] queueStates,
            WorkerState[] workerStates)
        {
            QueueStates = queueStates;
            WorkerStates = workerStates;
        }

        /// <summary>
        /// The state of all the queues on this subscription
        /// </summary>
        public QueueState[] QueueStates { get; set; }

        /// <summary>
        /// The state of all the workers in this subscription
        /// </summary>
        public WorkerState[] WorkerStates { get; set; }

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