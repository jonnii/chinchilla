using Chinchilla.Serializers;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class MessageSerializersSpecification
    {
        [Subject(typeof(MessageSerializers))]
        public class in_general : WithSubject<MessageSerializers>
        {
            It should_have_default = () =>
                Subject.Default.ShouldBeOfType<JsonMessageSerializer>();
        }

        [Subject(typeof(MessageSerializers))]
        public class when_setting_default : WithSubject<MessageSerializers>
        {
            Because of = () =>
                Subject.Default = new TestSerializer();

            It should_have_default = () =>
                Subject.Find("test-serializer").ShouldBeOfType<TestSerializer>();
        }

        [Subject(typeof(MessageSerializers))]
        public class when_setting_default_when_already_registered : WithSubject<MessageSerializers>
        {
            Establish context = () =>
                Subject.Register(new TestSerializer());

            Because of = () =>
                Subject.Default = new TestSerializer();

            It should_have_default = () =>
                Subject.Find("test-serializer").ShouldBeOfType<TestSerializer>();
        }

        [Subject(typeof(MessageSerializers))]
        public class when_replacing_existing_serializer_with_same_content_type : WithSubject<MessageSerializers>
        {
            Establish context = () =>
                Subject.Default = new TestSerializer();

            Because of = () =>
                Subject.Default = new AlternateTestSerializer();

            It should_have_default = () =>
                Subject.Default.ShouldBeOfType<AlternateTestSerializer>();

            It should_have_replaced_default_serializer_with_same_content_type = () =>
                Subject.Find("test-serializer").ShouldBeOfType<AlternateTestSerializer>();
        }

        public class TestSerializer : IMessageSerializer
        {
            public string ContentType { get { return "test-serializer"; } }

            public byte[] Serialize<TPayload>(IMessage<TPayload> message)
            {
                throw new System.NotImplementedException();
            }

            public IMessage<T> Deserialize<T>(byte[] message)
            {
                throw new System.NotImplementedException();
            }
        }

        public class AlternateTestSerializer : IMessageSerializer
        {
            public string ContentType { get { return "test-serializer"; } }

            public byte[] Serialize<TPayload>(IMessage<TPayload> message)
            {
                throw new System.NotImplementedException();
            }

            public IMessage<T> Deserialize<T>(byte[] message)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
