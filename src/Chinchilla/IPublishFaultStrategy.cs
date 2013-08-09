namespace Chinchilla
{
    public interface IPublishFaultStrategy
    {
        IPublishFaultAction<TMessage> OnFailedReceipt<TMessage>(IPublishReceipt receipt);
    }

    public interface IPublishFaultAction<TMessage>
    {
        void Run(IPublisher<TMessage> publisher, TMessage failedMessage);
    }
}