using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Serializers.JsonNET.Specifications
{
    public class JsonNetMessageSerializerSpecification
    {
        [Subject(typeof(JsonMessageSerializer))]
        public class in_general : WithSubject<JsonNetMessageSerializer>
        {
            It should_have_content_type = () =>
                Subject.ContentType.ShouldEqual("application/json");
        }

        [Subject(typeof(JsonMessageSerializer))]
        public class when_serializing : WithSubject<JsonMessageSerializer>
        {
            Because of = () =>
                serialized = Subject.Serialize(Message.Create(new InterestingFact()));

            It should_serialize = () =>
                serialized.Length.ShouldBeGreaterThan(0);

            static byte[] serialized;
        }

        [Subject(typeof(JsonMessageSerializer))]
        public class when_deserializing : WithSubject<JsonMessageSerializer>
        {
            Establish context = () =>
                serialized = Subject.Serialize(
                    Message.Create(new InterestingFact("Disney's Tangled is the 3rd most expensive film ever made...")));

            Because of = () =>
                deserialized = Subject.Deserialize<InterestingFact>(serialized);

            It should_deserialize = () =>
                deserialized.Body.FactBody.ShouldEqual("Disney's Tangled is the 3rd most expensive film ever made...");

            static byte[] serialized;

            static IMessage<InterestingFact> deserialized;
        }

        public class InterestingFact
        {
            public InterestingFact() { }

            public InterestingFact(string body)
            {
                FactBody = body;
            }

            public string FactBody { get; set; }
        }
    }

}
