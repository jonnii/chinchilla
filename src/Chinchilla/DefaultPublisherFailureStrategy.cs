namespace Chinchilla
{
    public class DefaultPublisherFailureStrategy<TMessage> : IPublisherFailureStrategy<TMessage>
    {
        public void OnFailure(IPublisher<TMessage> publisher, TMessage failedMessage, IPublishReceipt receipt)
        {

        }
    }
}