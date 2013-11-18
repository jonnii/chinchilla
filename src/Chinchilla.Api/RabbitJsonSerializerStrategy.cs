using System.IO;
using Newtonsoft.Json;
using RestSharp.Serializers;

namespace Chinchilla.Api
{
    public class RabbitJsonSerializerStrategy : ISerializer
    {
        private readonly Newtonsoft.Json.JsonSerializer serializer;

        public RabbitJsonSerializerStrategy()
        {
            ContentType = "application/json";
            serializer = new Newtonsoft.Json.JsonSerializer
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Include,
                DefaultValueHandling = DefaultValueHandling.Include,
                ContractResolver = new LowercaseContractResolver()
            };
        }

        public RabbitJsonSerializerStrategy(Newtonsoft.Json.JsonSerializer serializer)
        {
            ContentType = "application/json";
            this.serializer = serializer;
        }

        public string Serialize(object obj)
        {
            using (var stringWriter = new StringWriter())
            {
                using (var jsonTextWriter = new JsonTextWriter(stringWriter))
                {
                    jsonTextWriter.Formatting = Formatting.Indented;
                    jsonTextWriter.QuoteChar = '"';

                    serializer.Serialize(jsonTextWriter, obj);

                    var result = stringWriter.ToString();
                    return result;
                }
            }
        }

        public string DateFormat { get; set; }
        public string RootElement { get; set; }
        public string Namespace { get; set; }
        public string ContentType { get; set; }
    }
}