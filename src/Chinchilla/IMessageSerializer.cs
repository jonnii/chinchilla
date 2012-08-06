namespace Chinchilla
{
    /// <summary>
    /// A message serializer is responsible for serializing and deserializing
    /// messages into a binary format.
    /// </summary>
    public interface IMessageSerializer
    {
        byte[] Serialize<T>(IMessage<T> message);

        IMessage<T> Deserialize<T>(byte[] message);
    }
}