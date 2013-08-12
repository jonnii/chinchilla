namespace Chinchilla
{
    public interface IPublishFaultStrategy<TMessage>
    {
        void Run(IPublisher<TMessage> publisher, TMessage failedMessage, IPublishReceipt receipt);
    }
}