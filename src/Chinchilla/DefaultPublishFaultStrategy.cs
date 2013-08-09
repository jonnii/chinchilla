namespace Chinchilla
{
    public class DefaultPublishFaultStrategy : IPublishFaultStrategy
    {
        public IPublishFaultAction<TMessage> OnFailedReceipt<TMessage>(IPublishReceipt receipt)
        {
            return new NullPublishFaultAction<TMessage>();
        }
    }
}