using Chinchilla.Serializers;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications.Serializers
{
    public class JsonMessageSerializerSpecification
    {
        [Subject(typeof(JsonMessageSerializer))]
        public class in_general : WithSubject<JsonMessageSerializer>
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
                    Message.Create(
                        new InterestingFact("Disney's Tangled is the 3rd most expensive film ever made...", FactType.Food)));

            Because of = () =>
                deserialized = Subject.Deserialize<InterestingFact>(serialized);

            It should_deserialize_strings = () =>
                deserialized.Body.FactBody.ShouldEqual("Disney's Tangled is the 3rd most expensive film ever made...");

            It should_deserialize_enums = () =>
                deserialized.Body.FactType.ShouldEqual(FactType.Food);

            It should_deserialize_optional_enums = () =>
                deserialized.Body.OptionalFactType.ShouldEqual(FactType.Book);

            static byte[] serialized;

            static IMessage<InterestingFact> deserialized;
        }

        public enum FactType
        {
            None,
            Food,
            Film,
            Book
        }

        public class InterestingFact
        {
            public InterestingFact() { }

            public InterestingFact(string body, FactType factType)
            {
                FactBody = body;
                FactType = factType;
                OptionalFactType = FactType.Book;
            }

            public string FactBody { get; set; }

            public FactType FactType { get; set; }

            public FactType? OptionalFactType { get; set; }
        }
    }
}
