namespace Chinchilla
{
    /// <summary>
    /// The state of a queue
    /// </summary>
    public class QueueState
    {
        public QueueState(string name, long numAcceptedMessages, long numRejectedMessages, long numFailedMessages)
        {
            Name = name;
            NumAcceptedMessages = numAcceptedMessages;
            NumRejectedMessages = numRejectedMessages;
            NumFailedMessages = numFailedMessages;
        }

        /// <summary>
        /// The name of this queue
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The number of messages accepted by this subscription
        /// </summary>
        public long NumAcceptedMessages { get; private set; }

        /// <summary>
        /// The number of messages rejected by this subscription
        /// </summary>
        public long NumRejectedMessages { get; private set; }

        /// <summary>
        /// The number of failed messages processed by this subscription
        /// </summary>
        public long NumFailedMessages { get; private set; }
    }
}