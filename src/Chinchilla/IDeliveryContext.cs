namespace Chinchilla
{
    /// <summary>
    /// A delivery context gives you access to the context of a delivery
    /// </summary>
    public interface IDeliveryContext
    {
        /// <summary>
        /// The bus that this delivery context originated on
        /// </summary>
        IBus Bus { get; }

        /// <summary>
        /// Replies to the message in this delivery context with another message
        /// </summary>
        /// <typeparam name="TMessage">The type of message to reply with</typeparam>
        /// <param name="reply">The message to reply with</param>
        void Reply<TMessage>(TMessage reply)
            where TMessage : ICorrelated;
    }
}