using Machine.Fakes;
using Machine.Specifications;
using RabbitMQ.Client;

namespace Chinchilla.Specifications
{
    public class ModelReferenceSpecification
    {
        [Subject(typeof(ModelReference))]
        public class when_disposing : with_reference
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

        public class with_reference : WithFakes
        {
            Establish context = () =>
                Subject = new ModelReference(An<IModel>());

            protected static ModelReference Subject;
        }
    }
}
