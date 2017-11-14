using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Chinchilla.Serializers
{
    public class JsonMessageSerializer : IMessageSerializer
    {
        //private readonly IJsonSerializerStrategy strategy;

        public JsonMessageSerializer()
            : this(new DefaultMessageTypeFactory())
        {
        }

        public JsonMessageSerializer(IMessageTypeFactory messageTypeFactory)
        {
            //strategy = new ChinchillaSerializerStrategy(messageTypeFactory);
        }

        public string ContentType { get; } = "application/json";

        public byte[] Serialize<T>(IMessage<T> message)
        {
            var settings = new JsonSerializerSettings{
                Converters = new []{new StringEnumConverter()}
            };
            
            var serialized = JsonConvert.SerializeObject(message.Body, settings);
            return Encoding.UTF8.GetBytes(serialized);
        }

        public IMessage<T> Deserialize<T>(byte[] message)
        {
            var decoded = Encoding.UTF8.GetString(message);
            var deserialized = JsonConvert.DeserializeObject<T>(decoded);
            return Message.Create(deserialized);
        }
    }
}
