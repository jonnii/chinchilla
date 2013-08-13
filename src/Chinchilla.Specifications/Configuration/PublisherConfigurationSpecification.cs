using Chinchilla.Configuration;
using Chinchilla.Specifications.Messages;
using Chinchilla.Topologies;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications.Configuration
{
    public class PublisherConfigurationSpecification
    {
        [Subject(typeof(PublisherConfiguration<>))]
        public class in_general : WithSubject<PublisherConfiguration<TestMessage>>
        {
            It should_automatically_create_topology = () =>
                Subject.ShouldBuildTopology.ShouldBeTrue();

            It should_have_publisher_confirms_enabled_by_default = () =>
                Subject.ShouldConfirm.ShouldBeTrue();
        }

        [Subject(typeof(PublisherConfiguration<>))]
        public class when_building_default_topology : WithSubject<PublisherConfiguration<TestMessage>>
        {
            Because of = () =>
                messageTopology = Subject.BuildTopology(new Endpoint("endpointName", "messageType", 0));

            It should_build_topology = () =>
                messageTopology.ShouldNotBeNull();

            static IMessageTopology messageTopology;
        }

        [Subject(typeof(PublisherConfiguration<>))]
        public class when_building_router : WithSubject<PublisherConfiguration<TestMessage>>
        {
            Because of = () =>
                router = Subject.BuildRouter();

            It should_build_default_router = () =>
                router.ShouldBeOfType<DefaultRouter>();

            static IRouter router;
        }

        [Subject(typeof(PublisherConfiguration<>))]
        public class when_building_custom_router : WithSubject<PublisherConfiguration<TestMessage>>
        {
            Establish context = () =>
                Subject.RouteWith(new CustomRouter());

            Because of = () =>
                router = Subject.BuildRouter();

            It should_build_custom_router = () =>
                router.ShouldBeOfType<CustomRouter>();

            static IRouter router;
        }

        [Subject(typeof(PublisherConfiguration<>))]
        public class when_building_with_reply_to : WithSubject<PublisherConfiguration<TestMessage>>
        {
            Establish context = () =>
                Subject.ReplyTo("queue-name");

            Because of = () =>
                router = Subject.BuildRouter();

            It should_build_custom_router = () =>
                router.ReplyTo().ShouldEqual("queue-name");

            static IRouter router;
        }

        [Subject(typeof(PublisherConfiguration<>))]
        public class when_building_with_custom_fault_strategy : WithSubject<PublisherConfiguration<TestMessage>>
        {
            Establish context = () =>
                Subject.OnFailure<CustomPublisherFailureStrategy>(s => s.Property = "foo");

            Because of = () =>
                strategy = Subject.BuildFaultStrategy();

            It should_build_strategy = () =>
                ((CustomPublisherFailureStrategy)strategy).Property.ShouldEqual("foo");

            static IPublisherFailureStrategy<TestMessage> strategy;
        }

        [Subject(typeof(PublisherConfiguration<>))]
        public class when_building_with_no_custom_fault_strategy : WithSubject<PublisherConfiguration<TestMessage>>
        {
            Because of = () =>
               strategy = Subject.BuildFaultStrategy();

            It should_create_default_publish_fault_strategy = () =>
                strategy.ShouldBeOfType<DefaultPublisherFailureStrategy<TestMessage>>();

            static IPublisherFailureStrategy<TestMessage> strategy;
        }

        public class CustomPublisherFailureStrategy : IPublisherFailureStrategy<TestMessage>
        {
            public string Property { get; set; }

            public void OnFailure(IPublisher<TestMessage> publisher, TestMessage failedMessage, IPublishReceipt receipt)
            {

            }
        }

        public class CustomRouter : IRouter
        {
            public string Route<TMessage>(TMessage message)
            {
                return "#";
            }

            public string ReplyTo()
            {
                return string.Empty;
            }
        }
    }
}
