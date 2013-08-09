using Chinchilla.Configuration;
using Chinchilla.Topologies;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications.Configuration
{
    public class PublisherConfigurationSpecification
    {
        [Subject(typeof(PublisherConfiguration))]
        public class in_general : WithSubject<PublisherConfiguration>
        {
            It should_automatically_create_topology = () =>
                Subject.ShouldBuildTopology.ShouldBeTrue();

            It should_have_publisher_confirms_enabled_by_default = () =>
                Subject.ShouldConfirm.ShouldBeTrue();
        }

        [Subject(typeof(PublisherConfiguration))]
        public class when_building_default_topology : WithSubject<PublisherConfiguration>
        {
            Because of = () =>
                messageTopology = Subject.BuildTopology(new Endpoint("endpointName", "messageType", 0));

            It should_build_topology = () =>
                messageTopology.ShouldNotBeNull();

            static IMessageTopology messageTopology;
        }

        [Subject(typeof(PublisherConfiguration))]
        public class when_building_router : WithSubject<PublisherConfiguration>
        {
            Because of = () =>
                router = Subject.BuildRouter();

            It should_build_default_router = () =>
                router.ShouldBeOfType<DefaultRouter>();

            static IRouter router;
        }

        [Subject(typeof(PublisherConfiguration))]
        public class when_building_custom_router : WithSubject<PublisherConfiguration>
        {
            Establish context = () =>
                Subject.RouteWith(new CustomRouter());

            Because of = () =>
                router = Subject.BuildRouter();

            It should_build_custom_router = () =>
                router.ShouldBeOfType<CustomRouter>();

            static IRouter router;
        }

        [Subject(typeof(PublisherConfiguration))]
        public class when_building_with_reply_to : WithSubject<PublisherConfiguration>
        {
            Establish context = () =>
                Subject.ReplyTo("queue-name");

            Because of = () =>
                router = Subject.BuildRouter();

            It should_build_custom_router = () =>
                router.ReplyTo().ShouldEqual("queue-name");

            static IRouter router;
        }

        [Subject(typeof(PublisherConfiguration))]
        public class when_building_with_custom_fault_strategy : WithSubject<PublisherConfiguration>
        {
            Establish context = () =>
                Subject.OnPublishFaults<CustomPublishFaultStrategy>(s => s.Property = "foo");

            Because of = () =>
                strategy = Subject.BuildFaultStrategy();

            It should_build_strategy = () =>
                ((CustomPublishFaultStrategy)strategy).Property.ShouldEqual("foo");

            static IPublishFaultStrategy strategy;
        }

        [Subject(typeof(PublisherConfiguration))]
        public class when_building_with_no_custom_fault_strategy : WithSubject<PublisherConfiguration>
        {
            Because of = () =>
               strategy = Subject.BuildFaultStrategy();

            It should_create_default_publish_fault_strategy = () =>
                strategy.ShouldBeOfType<DefaultPublishFaultStrategy>();

            static IPublishFaultStrategy strategy;
        }

        public class CustomPublishFaultStrategy : IPublishFaultStrategy
        {
            public string Property { get; set; }

            public IPublishFaultAction<TMessage> OnFailedReceipt<TMessage>(IPublishReceipt receipt)
            {
                return null;
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
