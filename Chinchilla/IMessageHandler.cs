namespace Chinchilla
{
    public interface IMessageHandler<in TMessage>
    {
        void Handle(IMessage<TMessage> message);
    }
}