using Chinchilla.Specifications.Messages;
using Machine.Fakes;
using Machine.Specifications;
using RabbitMQ.Client;

namespace Chinchilla.Specifications
{
    public class BusSpecification
    {
        [Subject(typeof(Bus))]
        public class in_general : with_bus
        {
            It should_have_topology = () =>
                Subject.Topology.ShouldNotBeNull();
        }

        [Subject(typeof(Bus))]
        public class when_creating_publisher : with_bus
        {
            Because of = () =>
                publisher = Subject.CreatePublishChannel();

            It should_create_publisher = () =>
                publisher.ShouldNotBeNull();

            It should_create_new_model = () =>
                The<IConnection>().WasToldTo(c => c.CreateModel());

            static IPublishChannel publisher;
        }

        [Subject(typeof(Bus))]
        public class when_publishing : with_bus
        {
            Because of = () =>
                Subject.Publish(new TestMessage());

            It should_create_new_model = () =>
                The<IConnection>().WasToldTo(c => c.CreateModel());
        }

        public class with_bus : WithSubject<Bus>
        {
            Establish context = () =>
                The<IConnection>().WhenToldTo(c => c.CreateModel()).Return(An<IModel>());
        }
    }
}
