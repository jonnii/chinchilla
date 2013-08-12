using Chinchilla.Configuration;
using Chinchilla.Topologies;
using Chinchilla.Topologies.Model;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class PublisherFactorySpecification
    {
        [Subject(typeof(PublisherFactory))]
        public class when_building_publisher : with_publisher_factory
        {
            Because of = () =>
                Subject.Create<TestMessage>(modelReference, configuration);

            It should_build_router = () =>
                configuration.WasToldTo(c => c.BuildRouter());

            It should_build_fault_strategy = () =>
                configuration.WasToldTo(c => c.BuildFaultStrategy());
        }

        [Subject(typeof(PublisherFactory))]
        public class when_building_publisher_with_confirms : with_publisher_factory
        {
            Establish context = () =>
                configuration.WhenToldTo(c => c.ShouldConfirm).Return(true);

            Because of = () =>
                publisher = Subject.Create<TestMessage>(modelReference, configuration);

            It should_create_confirming_publisher = () =>
                publisher.ShouldBeOfType<ConfirmingPublisher<TestMessage>>();

            static IPublisher<TestMessage> publisher;
        }

        public class with_publisher_factory : WithSubject<PublisherFactory>
        {
            Establish context = () =>
            {
                modelReference = An<IModelReference>();
                configuration = An<IPublisherConfiguration<TestMessage>>();

                configuration.WhenToldTo(c => c.BuildRouter()).Return(An<IRouter>());
                configuration.WhenToldTo(c => c.BuildTopology(Param.IsAny<IEndpoint>()))
                    .Return(new MessageTopology { PublishExchange = An<IExchange>() });

                The<IMessageSerializers>().WhenToldTo(s => s.FindOrDefault(Param.IsAny<string>()))
                    .Return(An<IMessageSerializer>());
            };

            protected static IPublisherConfiguration<TestMessage> configuration;

            protected static IModelReference modelReference;
        }

        public class TestMessage { }
    }
}
