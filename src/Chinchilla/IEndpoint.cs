namespace Chinchilla
{
    /// <summary>
    /// An end point is either an exchange or a queue. Each 
    /// endpoint will map to its own consumer
    /// </summary>
    public interface IEndpoint
    {
        /// <summary>
        /// The name of the endpoint
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The type of the message received at this endpoint
        /// </summary>
        string MessageType { get; }

        /// <summary>
        /// Indicates the ordinal of this endpoint
        /// in a subscription.
        /// </summary>
        int Ordinal { get; }
    }
}