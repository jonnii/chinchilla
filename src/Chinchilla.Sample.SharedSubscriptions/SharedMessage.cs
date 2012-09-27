namespace Chinchilla.Sample.SharedSubscriptions
{
    public class SharedMessage
    {
        public SharedMessage() { }

        public SharedMessage(int id, MessageType type)
        {
            Id = id;
            MessageType = type;
        }

        public int Id { get; set; }

        public MessageType MessageType { get; set; }

        public override string ToString()
        {
            return string.Format("[SharedMessage Id={0}, MessageType={1}]", Id, MessageType);
        }
    }
}