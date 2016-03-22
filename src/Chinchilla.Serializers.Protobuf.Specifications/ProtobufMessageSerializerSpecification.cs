using Machine.Specifications;
using Google.Protobuf;
using Google.Protobuf.Examples.AddressBook;
using System.IO;

namespace Chinchilla.Serializers.Protobuf.Specifications
{
    public class ProtobufMessageSerializerSpecification
    {
        [Subject(typeof(ProtobufMessageSerializer))]
        public class when_serializing_deserializing
        {
            Establish context = () =>
               {
                   var registrar = new ProtobufRegistrar();
                   registrar.Register(AddressBook.Parser);
                   serializer = new ProtobufMessageSerializer(registrar);
               };

            Because of = () =>
            {
                var book = new AddressBook();
                var person = new Person() { Id = 1, Email = "furry@chinchilla.com", Name = "Peter" };
                book.People.Add(person);
                var serialized = serializer.Serialize(Message.Create(book));
                deserialized = serializer.Deserialize<AddressBook>(serialized);
            };

            It should_deserialize = () => deserialized.ShouldNotBeNull();

            It should_deserialize_body = () =>
            {
                deserialized.Body.People.Count.ShouldEqual(1);
                deserialized.Body.People[0].Name.ShouldEqual("Peter");
            };

            static ProtobufMessageSerializer serializer;

            static IMessage<Google.Protobuf.Examples.AddressBook.AddressBook> deserialized;
        }
        #region ProtoGenerated
        #endregion
    }
}
