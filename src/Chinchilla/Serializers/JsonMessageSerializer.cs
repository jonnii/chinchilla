using System.Text;
using Newtonsoft.Json;

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
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
        }

        public IMessage<T> Deserialize<T>(byte[] message)
        {
            return JsonConvert.DeserializeObject<Message<T>>(Encoding.UTF8.GetString(message));
        }
    }
}
