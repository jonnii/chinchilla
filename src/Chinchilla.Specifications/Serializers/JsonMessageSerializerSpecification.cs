using Chinchilla.Serializers;
using Machine.Specifications;

namespace Chinchilla.Specifications.Serializers
{
    public class JsonMessageSerializerSpecification
    {
        [Subject(typeof(JsonMessageSerializer))]
        public class in_general : with_serializer
        {
            It should_have_content_type = () =>
                Subject.ContentType.ShouldEqual("application/json");
        }

        [Subject(typeof(JsonMessageSerializer))]
        public class when_serializing : with_serializer
        {
            Because of = () =>
                serialized = Subject.Serialize(Message.Create(new InterestingFact()));

            It should_serialize = () =>
                serialized.Length.ShouldBeGreaterThan(0);

            static byte[] serialized;
        }

        [Subject(typeof(JsonMessageSerializer))]
        public class when_deserializing : with_serializer
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

        [Subject(typeof(JsonMessageSerializer))]
        public class when_deserializing_interface : with_serializer
        {
            Establish context = () =>
                serialized = Subject.Serialize(
                    Message.Create(
                        new InterestingFact("Disney's Tangled is the 3rd most expensive film ever made...", FactType.Food)));

            Because of = () =>
                deserialized = Subject.Deserialize<IInterestingFact>(serialized);

            It should_deserialize_strings = () =>
                deserialized.Body.FactBody.ShouldEqual("Disney's Tangled is the 3rd most expensive film ever made...");

            It should_deserialize_enums = () =>
                deserialized.Body.FactType.ShouldEqual(FactType.Food);

            static byte[] serialized;

            static IMessage<IInterestingFact> deserialized;
        }

        public class with_serializer
        {
            Establish context = () =>
            {
                Subject = new JsonMessageSerializer();
            };

            protected static JsonMessageSerializer Subject;
        }

        public enum FactType
        {
            None,
            Food,
            Film,
            Book
        }

        public interface IInterestingFact
        {
            string FactBody { get; set; }

            FactType FactType { get; set; }
        }

        public class InterestingFact : IInterestingFact
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
