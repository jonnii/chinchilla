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
            DeliveryStrategyState deliveryStrategyState)
        {
            NumAcceptedMessages = numAcceptedMessages;
            NumFailedMessages = numFailedMessages;
            DeliveryStrategyState = deliveryStrategyState;
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
        /// The delivery strategy state
        /// </summary>
        public DeliveryStrategyState DeliveryStrategyState { get; set; }
    }
}