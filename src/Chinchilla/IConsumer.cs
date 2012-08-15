namespace Chinchilla
{
    public interface IConsumer<in TMessage>
    {
        void Consume(TMessage message);
    }
}
