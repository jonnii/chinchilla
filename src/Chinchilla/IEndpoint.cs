namespace Chinchilla
{
    /// <summary>
    /// An end point is either an exchange or a queue 
    /// </summary>
    public interface IEndpoint
    {
        /// <summary>
        /// The name of the endpoint
        /// </summary>
        string Name { get; }

        string MessageType { get; }
    }
}