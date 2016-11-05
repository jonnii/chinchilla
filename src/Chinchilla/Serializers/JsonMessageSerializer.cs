using System.Text;

namespace Chinchilla.Serializers
{
    public class JsonMessageSerializer : IMessageSerializer
    {
        private readonly IJsonSerializerStrategy strategy;

        public JsonMessageSerializer()
            : this(new NullMessageTypeFactory())
        {
        }

        public JsonMessageSerializer(IMessageTypeFactory messageTypeFactory)
        {
            strategy = new ChinchillaSerializerStrategy(messageTypeFactory);
        }

        public string ContentType { get; } = "application/json";

        public byte[] Serialize<T>(IMessage<T> message)
        {
            var serialized = SimpleJson.SerializeObject(message.Body, strategy);
            return Encoding.UTF8.GetBytes(serialized);
        }

        public IMessage<T> Deserialize<T>(byte[] message)
        {
            var decoded = Encoding.UTF8.GetString(message);
            var deserialized = SimpleJson.DeserializeObject<T>(decoded, strategy);
            return Message.Create(deserialized);
        }
    }
}
