using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class DefaultRouterSpecification
    {
        [Subject(typeof(DefaultRouter))]
        public class when_routing_message : WithSubject<DefaultRouter>
        {
            Because of = () =>
                routingKey = Subject.Route(new TestMessage());

            It should_create_default_key = () =>
                routingKey.ShouldEqual("#");

            static string routingKey;
        }

        [Subject(typeof(DefaultRouter))]
        public class when_routing_message_with_routing_key : WithSubject<DefaultRouter>
        {
            Because of = () =>
                routingKey = Subject.Route(new TestMessageWithRoutingKey());

            It should_use_routing_key_property = () =>
                routingKey.ShouldEqual("route-this-to-mars");

            static string routingKey;
        }

        public class TestMessage { }

        public class TestMessageWithRoutingKey : IHasRoutingKey
        {
            public string RoutingKey { get { return "route-this-to-mars"; } }
        }
    }
}