namespace Chinchilla
{
    /// <summary>
    /// A publisher failure strategy is used to handle a failed publish, this will only
    /// work when a publisher has confirms enabled.
    /// </summary>
    public interface IPublisherFailureStrategy<TMessage>
    {
        /// <summary>
        /// On failured is called when a message fails to publish
        /// </summary>
        /// <param name="publisher">The publisher that failed</param>
        /// <param name="failedMessage">The original failed message</param>
        /// <param name="receipt">The publish receipt for this message</param>
        void OnFailure(IPublisher<TMessage> publisher, TMessage failedMessage, IPublishReceipt receipt);
    }
}