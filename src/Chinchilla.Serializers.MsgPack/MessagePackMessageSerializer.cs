using MsgPack;

namespace Chinchilla.Serializers.MsgPack
{
    public class MessagePackMessageSerializer : IMessageSerializer
    {
        private readonly ObjectPacker packer;

        public MessagePackMessageSerializer()
        {
            packer = new ObjectPacker();
        }

        public string ContentType
        {
            get { return "application/x-msgpack"; }
        }

        public byte[] Serialize<T>(IMessage<T> message)
        {

            var packed = packer.Pack(message);
            return packed;
        }

        public IMessage<T> Deserialize<T>(byte[] message)
        {
            var unpacked = packer.Unpack<Message<T>>(message);
            return unpacked;
        }
    }
}
