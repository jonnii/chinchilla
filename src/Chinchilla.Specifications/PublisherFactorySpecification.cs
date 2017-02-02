using Chinchilla.Configuration;
using Chinchilla.Topologies;
using Chinchilla.Topologies.Model;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    [Subject(typeof(PublisherFactory))]
    class PublisherFactorySpecification : WithSubject<PublisherFactory>
    {
        static IPublisherConfiguration<TestMessage> configuration;

        static IModelReference reference;

        static IPublisher<TestMessage> publisher;

        Establish context = () =>
        {
            reference = An<IModelReference>();
            configuration = An<IPublisherConfiguration<TestMessage>>();

            configuration.WhenToldTo(c => c.BuildRouter()).Return(An<IRouter>());
            configuration.WhenToldTo(c => c.BuildTopology(Param.IsAny<IEndpoint>()))
                .Return(new MessageTopology { PublishExchange = An<IExchange>() });

            The<IMessageSerializers>().WhenToldTo(s => s.FindOrDefault(Param.IsAny<string>()))
                .Return(An<IMessageSerializer>());
        };

        class when_building_publisher
        {
            Because of = () =>
                Subject.Create(reference, configuration);

            It should_build_router = () =>
                configuration.WasToldTo(c => c.BuildRouter());

            It should_build_fault_strategy = () =>
                configuration.WasToldTo(c => c.BuildFaultStrategy());
        }

        class when_building_publisher_with_confirms
        {
            Establish context = () =>
                configuration.WhenToldTo(c => c.ShouldConfirm).Return(true);

            Because of = () =>
                publisher = Subject.Create<TestMessage>(reference, configuration);

            It should_create_confirming_publisher = () =>
                publisher.ShouldBeAssignableTo<ConfirmingPublisher<TestMessage>>();

        }

        class when_disposing_of_publisher
        {
            Because of = () =>
                Subject.Create(reference, configuration).Dispose();

            It should_not_be_tracking_publishers = () =>
                Subject.Tracked.ShouldBeEmpty();
        }

        public class TestMessage { }
    }
}
