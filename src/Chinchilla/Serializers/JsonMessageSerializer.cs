using System.Text;

namespace Chinchilla.Serializers
{
    public class JsonMessageSerializer : IMessageSerializer
    {
        public string ContentType
        {
            get { return "application/json"; }
        }

        public byte[] Serialize<T>(IMessage<T> message)
        {
            var serialized = SimpleJson.SerializeObject(message);
            return Encoding.UTF8.GetBytes(serialized);
        }

        public IMessage<T> Deserialize<T>(byte[] message)
        {
            var decoded = Encoding.UTF8.GetString(message);
            return SimpleJson.DeserializeObject<Message<T>>(decoded);
        }
    }
}
