using MsgPack.Serialization;

namespace Chinchilla.Serializers.MsgPack
{
    public class MessagePackMessageSerializer : IMessageSerializer
    {
        public static class SerializerInstance<T>
        {
            public static readonly MessagePackSerializer<MessageWrapper<T>> CachedInstance =
                MessagePackSerializer.Get<MessageWrapper<T>>();
        }

        public class MessageWrapper<T>
        {
            public T Body { get; set; }
        }

        public string ContentType => "application/x-msgpack";

        public byte[] Serialize<T>(IMessage<T> message)
        {
            var packer = SerializerInstance<T>.CachedInstance;
            return packer.PackSingleObject(new MessageWrapper<T> { Body = message.Body });
        }

        public IMessage<T> Deserialize<T>(byte[] message)
        {
            var packer = SerializerInstance<T>.CachedInstance;
            var wrapped = packer.UnpackSingleObject(message);
            return new Message<T>(wrapped.Body);
        }
    }
}
