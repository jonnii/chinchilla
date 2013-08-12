namespace Chinchilla
{
    public class DefaultPublishFaultStrategy<TMessage> : IPublishFaultStrategy<TMessage>
    {
        public void Run(IPublisher<TMessage> publisher, TMessage failedMessage, IPublishReceipt receipt)
        {

        }
    }
}