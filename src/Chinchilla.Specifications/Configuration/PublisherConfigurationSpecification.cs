using Chinchilla.Configuration;
using Chinchilla.Topologies;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications.Configuration
{
    public class PublisherConfigurationSpecification
    {
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
