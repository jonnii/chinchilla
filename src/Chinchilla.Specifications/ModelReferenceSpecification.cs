using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class ModelReferenceSpecification
    {
        [Subject(typeof(ModelReference))]
        public class when_disposing : WithSubject<ModelReference>
        {
            Establish context = () =>
            {
                Subject.Disposed += (o, e) => raised = true;
            };

            Because of = () =>
                Subject.Dispose();

            It should_raise_disposed_event = () =>
                raised.ShouldBeTrue();

            static bool raised;
        }
    }
}
