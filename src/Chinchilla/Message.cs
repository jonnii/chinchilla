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
        public Message() { }

        public Message(T body)
        {
            Body = body;
        }

        public T Body { get; set; }
    }
}