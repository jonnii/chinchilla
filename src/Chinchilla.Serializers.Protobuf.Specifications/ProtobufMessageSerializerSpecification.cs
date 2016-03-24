using Google.Protobuf.Examples.AddressBook;
using Machine.Specifications;

namespace Chinchilla.Serializers.Protobuf.Specifications
{
    public class ProtobufMessageSerializerSpecification
    {
        [Subject(typeof(ProtobufMessageSerializer))]
        public class when_serializing_deserializing
        {
            private Establish context = () => serializer = ProtobufMessageSerializer.Create(s => s.Register(AddressBook.Parser));

            private Because of = () =>
             {
                 var book = new AddressBook();
                 var person = new Person() { Id = 1, Email = "furry@chinchilla.com", Name = "Peter" };
                 book.People.Add(person);
                 var serialized = serializer.Serialize(Message.Create(book));
                 deserialized = serializer.Deserialize<AddressBook>(serialized);
             };

            private It should_deserialize = () => deserialized.ShouldNotBeNull();

            private It should_deserialize_body = () =>
            {
                deserialized.Body.People.Count.ShouldEqual(1);
                deserialized.Body.People[0].Name.ShouldEqual("Peter");
            };

            private static IMessageSerializer serializer;

            private static IMessage<AddressBook> deserialized;
        }
    }
}