using Machine.Fakes;
using Machine.Specifications;
using RabbitMQ.Client;

namespace Chinchilla.Specifications
{
    public class ModelFactorySpecification
    {
        [Subject(typeof(ModelFactory))]
        public class when_creating_model : WithSubject<ModelFactory>
        {
            Because of = () =>
                reference = (ModelReference)Subject.CreateModel();

            It should_be_tracking_reference = () =>
                Subject.IsTracking(reference).ShouldBeTrue();

            static ModelReference reference;
        }

        [Subject(typeof(ModelFactory))]
        public class when_disposing_of_model_reference : WithSubject<ModelFactory>
        {
            Establish context = () =>
                reference = (ModelReference)Subject.CreateModel();

            Because of = () =>
                reference.Dispose();

            It should_untrack_reference = () =>
                Subject.IsTracking(reference).ShouldBeFalse();

            static ModelReference reference;
        }

        [Subject(typeof(ModelFactory))]
        public class when_disposing : WithSubject<ModelFactory>
        {
            Because of = () =>
                Subject.Dispose();

            It should_dispose_connection = () =>
                The<IConnection>().WasToldTo(c => c.Dispose());
        }
    }
}
