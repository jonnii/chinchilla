namespace Chinchilla
{
    /// <summary>
    /// A router is responsible for creating a routing key for a message
    /// </summary>
    public interface IRouter
    {
        /// <summary>
        /// Gets a routing key for a given message
        /// </summary>
        string Route<TMessage>(TMessage message);

        /// <summary>
        /// Gets the reply to address for this router
        /// </summary>
        /// <returns>The reply to address for this router</returns>
        string ReplyTo();
    }
}