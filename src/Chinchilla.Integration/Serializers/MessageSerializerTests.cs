using Chinchilla.Serializers;
using Chinchilla.Serializers.MsgPack;
using Xunit;

namespace Chinchilla.Integration.Serializers
{
    public class MessageSerializerTests
    {
        [Fact]
        public void ValidateDefaultJsonSerializer()
        {
            TestSerializer<JsonMessageSerializer>();
        }

        [Fact]
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

            Assert.Equal(expected.SimpleString, actual.SimpleString);
            Assert.Equal(expected.SimpleEnum, actual.SimpleEnum);
            Assert.Equal(expected.SimpleInteger, actual.SimpleInteger);
            Assert.Equal(expected.SimpleDate, actual.SimpleDate);

            Assert.Equal(expected.NullableEnum, actual.NullableEnum);
            Assert.Equal(expected.NullableEnumWithValue, actual.NullableEnumWithValue);
        }
    }
}
