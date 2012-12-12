using Machine.Specifications;

namespace Chinchilla.Serializers.MsgPack.Specifications
{
    public class MessagePackMessageSerializerSpecification
    {
        [Subject(typeof(MessagePackMessageSerializer))]
        public class when_serializing_deserializing
        {
            Establish context = () =>
                serializer = new MessagePackMessageSerializer();

            Because of = () =>
            {
                var serialized = serializer.Serialize(Message.Create(new Payload { Foo = 5, Bar = "hi" }));
                deserialized = serializer.Deserialize<Payload>(serialized);
            };

            It should_deserialize = () =>
                deserialized.ShouldNotBeNull();

            It should_deserialize_body = () =>
            {
                deserialized.Body.Foo.ShouldEqual(5);
                deserialized.Body.Bar.ShouldEqual("hi");
            };

            static MessagePackMessageSerializer serializer;

            static IMessage<Payload> deserialized;
        }

        public class Payload
        {
            public int Foo { get; set; }

            public string Bar { get; set; }
        }
    }
}
