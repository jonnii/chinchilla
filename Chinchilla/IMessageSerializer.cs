namespace Chinchilla
{
    public interface IMessageSerializer
    {
        byte[] Serialize<T>(IMessage<T> message);
    }
}