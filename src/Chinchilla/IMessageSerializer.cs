namespace Chinchilla
{
    /// <summary>
    /// A message serializer is responsible for serializing and deserializing
    /// messages into a binary format.
    /// </summary>
    public interface IMessageSerializer
    {
        /// <summary>
        /// The content type of this message serializer
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// Converts a message into a byte array
        /// </summary>
        /// <typeparam name="TPayload">The type of the payload of this message</typeparam>
        /// <param name="message">A message</param>
        /// <returns>A serialized byte array</returns>
        byte[] Serialize<TPayload>(IMessage<TPayload> message);

        /// <summary>
        /// Deserializes the given byte array into a message
        /// </summary>
        /// <typeparam name="T">The type of the message payload</typeparam>
        /// <param name="message">A message byte array</param>
        /// <returns>A message</returns>
        IMessage<T> Deserialize<T>(byte[] message);
    }
}