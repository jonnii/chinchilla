namespace Chinchilla
{
    public static class Message
    {
        public static Message<T> Create<T>(T body)
        {
            return new Message<T>(body);
        }
    }

    public class Message<T> : IMessage<T>
    {
        public Message(T body)
        {
            Properties = new MessageProperties();
            Body = body;
        }

        public MessageProperties Properties { get; private set; }

        public T Body { get; private set; }
    }
}