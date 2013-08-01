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

            var expected = ComplexMessage.Default;

            var serialized = serializer.Serialize(Message.Create(expected));
            var deserialized = serializer.Deserialize<ComplexMessage>(serialized);
            var actual = deserialized.Body;

            Assert.That(actual.SimpleString, Is.EqualTo(expected.SimpleString));
            Assert.That(actual.SimpleEnum, Is.EqualTo(expected.SimpleEnum));
            Assert.That(actual.SimpleInteger, Is.EqualTo(expected.SimpleInteger));
            Assert.That(actual.SimpleDate, Is.EqualTo(expected.SimpleDate));

            Assert.That(actual.NullableEnum, Is.EqualTo(expected.NullableEnum));
            Assert.That(actual.NullableEnumWithValue, Is.EqualTo(expected.NullableEnumWithValue));
        }
    }
}
