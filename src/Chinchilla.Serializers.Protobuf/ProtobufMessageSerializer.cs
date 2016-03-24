using Google.Protobuf;
using System;
using System.Collections.Generic;

namespace Chinchilla.Serializers.Protobuf
{
    public class ProtobufMessageSerializer : IMessageSerializer
    {
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

        private Dictionary<Type, SerializerFunc> _cache = new Dictionary<Type, SerializerFunc>();

        private ProtobufMessageSerializer()
        {
        }

        public void Register<T>(MessageParser<T> parser) where T : Google.Protobuf.IMessage<T>
        {
            var toBytes = new Func<byte[], object>(b => parser.ParseFrom(b));
            var fromBytes = new Func<object, byte[]>(o => MessageExtensions.ToByteArray((T)o));
            if (!_cache.ContainsKey(typeof(T)))
            {
                _cache.Add(typeof(T), new SerializerFunc(toBytes, fromBytes));
            }
        }

        public static IMessageSerializer Create(Action<ProtobufMessageSerializer> create)
        {
            var s = new ProtobufMessageSerializer();
            create(s);
            return s;
        }

        public string ContentType { get; } = "application/x-protobuf";

        public IMessage<T> Deserialize<T>(byte[] message)
        {
            return Message.Create((T)_cache[typeof(T)].FromProto(message));
        }

        public byte[] Serialize<T>(IMessage<T> message)
        {
            return _cache[typeof(T)].ToProto(message.Body);
        }
    }
}