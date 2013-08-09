namespace Chinchilla
{
    public class NullPublishFaultAction<TMessage> : IPublishFaultAction<TMessage>
    {
        public void Run(IPublisher<TMessage> publisher, TMessage failedMessage)
        {

        }
    }
}