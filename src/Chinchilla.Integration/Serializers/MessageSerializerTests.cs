using Chinchilla.Serializers;
using Chinchilla.Serializers.JsonNET;
using Chinchilla.Serializers.MsgPack;
using NUnit.Framework;

namespace Chinchilla.Integration.Serializers
{
    [TestFixture]
    public class MessageSerializerTests
    {
        [Test]
        public void ValidateDefaultJsonSerializer()
        {
            TestSerializer<JsonMessageSerializer>();
        }

        [Test]
        public void ValidateJsonNetSerializer()
        {
            TestSerializer<JsonNetMessageSerializer>();
        }

        [Test]
        public void ValidateMessagePackSerializer()
        {
            TestSerializer<MessagePackMessageSerializer>();
        }

        private void TestSerializer<T>()
            where T : IMessageSerializer, new()
        {
            var serializer = new T();

            var expected = new ComplexMessage("simple_string");

            var serialized = serializer.Serialize(Message.Create(expected));
            var deserialized = serializer.Deserialize<ComplexMessage>(serialized);
            var actual = deserialized.Body;

            Assert.That(actual.SimpleString, Is.EqualTo(expected.SimpleString));
        }
    }
}
