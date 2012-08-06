namespace Chinchilla
{
    /// <summary>
    /// A publisher is used to publish a message to an exchange.
    /// </summary>
    public interface IPublisher
    {
        /// <summary>
        /// Publishes a message to an exchange
        /// </summary>
        /// <typeparam name="T">The type of message to publish</typeparam>
        void Publish<T>(T message);
    }
}