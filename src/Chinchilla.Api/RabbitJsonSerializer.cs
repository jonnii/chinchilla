using RestSharp;
using RestSharp.Serializers;

namespace Chinchilla.Api
{
    public class RabbitJsonSerializer : ISerializer
    {
        public RabbitJsonSerializer()
        {
            ContentType = "application/json";
        }

        public string Serialize(object obj)
        {
            return SimpleJson.SerializeObject(obj, new RabbitJsonSerializerStrategy());
        }

        public string RootElement { get; set; }
        public string Namespace { get; set; }
        public string DateFormat { get; set; }
        public string ContentType { get; set; }
    }
}