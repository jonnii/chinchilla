using Google.Protobuf;
using System;
using System.Collections.Generic;

namespace Chinchilla.Serializers.Protobuf
{
    public class ProtobufMessageSerializer : IMessageSerializer
    {
        public static IMessageSerializer Create(Action<ProtobufMessageSerializer> onCreate)
        {
            var serializer = new ProtobufMessageSerializer();
            onCreate(serializer);
            return serializer;
        }

        private readonly Dictionary<Type, SerializerFunc> cache = new Dictionary<Type, SerializerFunc>();

        private ProtobufMessageSerializer()
        {
        }

        public string ContentType { get; } = "application/x-protobuf";

        public void Register<T>(MessageParser<T> parser)
            where T : Google.Protobuf.IMessage<T>
        {
            var toBytes = new Func<byte[], object>(b => parser.ParseFrom(b));
            var fromBytes = new Func<object, byte[]>(o => ((T)o).ToByteArray());

            if (!cache.ContainsKey(typeof(T)))
            {
                cache.Add(typeof(T), new SerializerFunc(toBytes, fromBytes));
            }
        }

        public IMessage<T> Deserialize<T>(byte[] message)
        {
            return Message.Create((T)cache[typeof(T)].FromProto(message));
        }

        public byte[] Serialize<T>(IMessage<T> message)
        {
            return cache[typeof(T)].ToProto(message.Body);
        }

        private class SerializerFunc
        {
            public SerializerFunc(Func<byte[], object> fromProto, Func<object, byte[]> toProto)
            {
                FromProto = fromProto;
                ToProto = toProto;
            }

            internal Func<byte[], object> FromProto { get; }

            internal Func<object, byte[]> ToProto { get; }
        }
    }
}