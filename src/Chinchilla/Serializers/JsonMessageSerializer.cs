﻿using System.Text;
using Newtonsoft.Json;

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
            var serialized = JsonConvert.SerializeObject(message.Body);
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
