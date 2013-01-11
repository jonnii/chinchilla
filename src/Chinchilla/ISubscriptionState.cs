namespace Chinchilla
{
    /// <summary>
    /// A subscription state is a breakdown of what the current subscription is doing
    /// </summary>
    public class SubscriptionState
    {
        public SubscriptionState(
            long numAcceptedMessages,
            long numFailedMessages,
            WorkerState[] workerStates)
        {
            NumAcceptedMessages = numAcceptedMessages;
            NumFailedMessages = numFailedMessages;
            WorkerStates = workerStates;
        }

        /// <summary>
        /// The number of messages accepted by this subscription
        /// </summary>
        public long NumAcceptedMessages { get; private set; }

        /// <summary>
        /// The number of failed messages processed by this subscription
        /// </summary>
        public long NumFailedMessages { get; private set; }

        /// <summary>
        /// The state of all the workers in this subscription
        /// </summary>
        public WorkerState[] WorkerStates { get; set; }
    }
}