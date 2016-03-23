using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Chinchilla.Serializers.Protobuf
{
    
    public class ProtobufMessageSerializer : IMessageSerializer
    {
        public class MessageWrapper<T> : IMessage<T>
        {
            public MessageWrapper(T body)
            {
                Body = body;
            }
            public T Body { get; }
        }

        private IProtobufRegistrar _protobufRegistrar;

        public ProtobufMessageSerializer(IProtobufRegistrar protobufRegistrar)
        {
            _protobufRegistrar = protobufRegistrar;
        }

        public string ContentType
        {
            get
            {
                return "application/x-protobuf";
            }
        }

        public IMessage<T> Deserialize<T>(byte[] message)
        {
            var typeSerializer = _protobufRegistrar.FromProto<T>();
            return new MessageWrapper<T>((T) typeSerializer(message));
        }

        public byte[] Serialize<T>(IMessage<T> message)
        {
            var typeSerializer = _protobufRegistrar.ToProto<T>();
            return typeSerializer(message.Body);
        }
    }
}
