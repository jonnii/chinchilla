namespace Chinchilla
{
    public interface IMessage<out T>
    {
        MessageProperties Properties { get; }

        T Body { get; }
    }
}