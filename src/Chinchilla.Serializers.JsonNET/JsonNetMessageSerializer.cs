using System.Text;
using Newtonsoft.Json;

namespace Chinchilla.Serializers.JsonNET
{
    public class JsonNetMessageSerializer : IMessageSerializer
    {
        public string ContentType
        {
            get { return "application/json"; }
        }

        public byte[] Serialize<T>(IMessage<T> message)
        {
            var serialized = JsonConvert.SerializeObject(message);
            return Encoding.UTF8.GetBytes(serialized);
        }

        public IMessage<T> Deserialize<T>(byte[] message)
        {
            var decoded = Encoding.UTF8.GetString(message);
            return JsonConvert.DeserializeObject<Message<T>>(decoded);
        }
    }
}
