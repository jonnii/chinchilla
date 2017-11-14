using System;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Chinchilla.Serializers
{
    public class JsonMessageSerializer : IMessageSerializer
    {
        private readonly JsonSerializerSettings settings;

        public JsonMessageSerializer()
            : this(new DefaultMessageTypeFactory())
        {
        }

        public JsonMessageSerializer(IMessageTypeFactory messageTypeFactory)
        {
            settings = new JsonSerializerSettings{
                Converters = new []{ new StringEnumConverter() },
                ContractResolver = new MagicContractResolver(messageTypeFactory)
            };
        }

        public class MagicContractResolver : DefaultContractResolver
        {
            private readonly IMessageTypeFactory messageTypeFactory;

            public MagicContractResolver(IMessageTypeFactory messageTypeFactory)
            {
                this.messageTypeFactory = messageTypeFactory;
            }

            protected override JsonObjectContract CreateObjectContract(Type objectType)
            {
                JsonObjectContract contract = base.CreateObjectContract(objectType);

                if (!objectType.GetTypeInfo().IsInterface)
                {
                    return contract;
                }

                contract.DefaultCreator = messageTypeFactory.GetTypeFactory(objectType);

                return contract;
            }
        }

        public string ContentType { get; } = "application/json";

        public byte[] Serialize<T>(IMessage<T> message)
        {
            var serialized = JsonConvert.SerializeObject(message.Body, settings);
            return Encoding.UTF8.GetBytes(serialized);
        }

        public IMessage<T> Deserialize<T>(byte[] message)
        {
            var decoded = Encoding.UTF8.GetString(message);
            var deserialized = JsonConvert.DeserializeObject<T>(decoded, settings);
            return Message.Create(deserialized);
        }
    }
}
