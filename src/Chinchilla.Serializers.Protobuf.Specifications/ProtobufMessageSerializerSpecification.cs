using Google.Protobuf.Examples.AddressBook;
using Machine.Specifications;

namespace Chinchilla.Serializers.Protobuf.Specifications
{
    class ProtobufMessageSerializerSpecification
    {
        [Subject(typeof(ProtobufMessageSerializer))]
        class when_serializing_deserializing
        {
            Establish context = () =>
            {
                book = new AddressBook();
                var person = new Person
                {
                    Id = 1,
                    Email = "furry@chinchilla.com",
                    Name = "Peter"
                };

                book.People.Add(person);

                serializer = ProtobufMessageSerializer.Create(s => s.Register(AddressBook.Parser));
            };

            Because of = () =>
            {
                var serialized = serializer.Serialize(Message.Create(book));
                deserialized = serializer.Deserialize<AddressBook>(serialized);
            };

            It should_deserialize = () =>
                deserialized.ShouldNotBeNull();

            It should_deserialize_body = () =>
            {
                deserialized.Body.People.Count.ShouldEqual(1);
                deserialized.Body.People[0].Name.ShouldEqual("Peter");
            };

            static IMessageSerializer serializer;

            static IMessage<AddressBook> deserialized;

            static AddressBook book;
        }
    }
}