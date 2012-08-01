namespace Chinchilla
{
    public interface IPublisher
    {
        void Publish<T>(T message);
    }
}