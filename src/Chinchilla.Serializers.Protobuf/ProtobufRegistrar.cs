using System;
using System.Collections.Generic;
using Google.Protobuf;

namespace Chinchilla.Serializers.Protobuf
{
    public interface IProtobufRegistrar
    {
        void Register<T>(MessageParser<T> parser) where T : Google.Protobuf.IMessage<T>, new();
        Func<byte[], object> FromProto<T>();
        Func<object, byte[]> ToProto<T>();
    }

    public class ProtobufRegistrar : IProtobufRegistrar
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

        private Dictionary<Type, SerializerFunc> cache = new Dictionary<Type, SerializerFunc>();

        public void Register<T>(MessageParser<T> parser) where T : Google.Protobuf.IMessage<T>, new()
        {
            var toBytes = new Func<byte[], object>(b => parser.ParseFrom(b));
            var fromBytes = new Func<object, byte[]>(o => MessageExtensions.ToByteArray((T)o));
            if (!cache.ContainsKey(typeof(T)))
            {
                cache.Add(typeof(T), new SerializerFunc(toBytes, fromBytes));
            }
        }

        public Func<byte[], object> FromProto<T>()
        {
            return cache[typeof(T)].FromProto; 
        }

        public Func<object, byte[]> ToProto<T>()
        {
            return cache[typeof(T)].ToProto;
        }
    }
}
