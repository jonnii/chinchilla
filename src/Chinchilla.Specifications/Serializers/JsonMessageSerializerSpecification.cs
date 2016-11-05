using System.Text;
using Chinchilla.Serializers;
using Machine.Specifications;

namespace Chinchilla.Specifications.Serializers
{
    [Subject(typeof(JsonMessageSerializer))]
    public class JsonMessageSerializerSpecification
    {
        static JsonMessageSerializer serializer;

        Establish context = () =>
            serializer = new JsonMessageSerializer();

        class in_general
        {
            It should_have_content_type = () =>
                serializer.ContentType.ShouldEqual("application/json");
        }

        class when_serializing
        {
            Because of = () =>
                serialized = serializer.Serialize(Message.Create(new InterestingFact()));

            It should_serialize = () =>
                serialized.Length.ShouldBeGreaterThan(0);

            It should_not_serialize_body = () =>
                Encoding.ASCII.GetString(serialized).ShouldNotContain("\"Body\"");

            static byte[] serialized;
        }

        class when_deserializing
        {
            Establish context = () =>
                serialized = serializer.Serialize(
                    Message.Create(
                        new InterestingFact("Disney's Tangled is the 3rd most expensive film ever made...", FactType.Food)));

            Because of = () =>
                deserialized = serializer.Deserialize<InterestingFact>(serialized);

        //     It should_deserialize_strings = () =>
        //         deserialized.Body.FactBody.ShouldEqual("Disney's Tangled is the 3rd most expensive film ever made...");

        //     It should_deserialize_enums = () =>
        //         deserialized.Body.FactType.ShouldEqual(FactType.Food);

        //     It should_deserialize_optional_enums = () =>
        //         deserialized.Body.OptionalFactType.ShouldEqual(FactType.Book);

             static byte[] serialized;

             static IMessage<InterestingFact> deserialized;
        }

        class when_deserializing_interface
        {
            Establish context = () =>
                serialized = serializer.Serialize(
                    Message.Create(
                        new InterestingFact("Disney's Tangled is the 3rd most expensive film ever made...", FactType.Food)));

            Because of = () =>
                deserialized = serializer.Deserialize<IInterestingFact>(serialized);

        //     It should_deserialize_strings = () =>
        //         deserialized.Body.FactBody.ShouldEqual("Disney's Tangled is the 3rd most expensive film ever made...");

        //     It should_deserialize_enums = () =>
        //         deserialized.Body.FactType.ShouldEqual(FactType.Food);

             static byte[] serialized;

             static IMessage<IInterestingFact> deserialized;
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
