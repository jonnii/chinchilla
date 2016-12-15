using System.Text;
using Newtonsoft.Json;

namespace Chinchilla.Serializers.JsonNET
{
    public class JsonNetMessageSerializer : IMessageSerializer
    {
        private readonly JsonSerializerSettings serializerSettings;

        public JsonNetMessageSerializer()
        {
        }

        public JsonNetMessageSerializer(JsonSerializerSettings serializerSettings)
        {
            this.serializerSettings = serializerSettings;
        }

        public string ContentType { get; } = "application/json";

        public byte[] Serialize<T>(IMessage<T> message)
        {
            var serialized = JsonConvert.SerializeObject(message.Body, serializerSettings);
            return Encoding.UTF8.GetBytes(serialized);
        }

        public IMessage<T> Deserialize<T>(byte[] message)
        {
            var decoded = Encoding.UTF8.GetString(message);
            var deserialized = JsonConvert.DeserializeObject<T>(decoded, serializerSettings);
            return Message.Create(deserialized);
        }
    }
}
