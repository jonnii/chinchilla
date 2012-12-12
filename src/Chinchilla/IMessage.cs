namespace Chinchilla
{
    public interface IMessage<out T>
    {
        T Body { get; }
    }
}