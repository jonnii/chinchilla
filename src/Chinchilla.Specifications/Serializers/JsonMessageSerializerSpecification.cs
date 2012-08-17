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
    }
}
