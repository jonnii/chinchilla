namespace Chinchilla
{
    /// <summary>
    /// A marker interface, not to be used directly
    /// </summary>
    public interface IConsumer
    {

    }

    /// <summary>
    /// A message consumer
    /// </summary>
    /// <typeparam name="TMessage">The type of message to be consumed</typeparam>
    public interface IConsumer<in TMessage> : IConsumer
    {
        /// <summary>
        /// Consumes a message
        /// </summary>
        void Consume(TMessage message, IMessageContext messageContext);
    }
}
