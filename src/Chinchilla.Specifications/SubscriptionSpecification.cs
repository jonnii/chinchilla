using System.Collections.Concurrent;
using Chinchilla.Topologies.Model;
using Machine.Fakes;
using Machine.Specifications;
using RabbitMQ.Client.Events;

namespace Chinchilla.Specifications
{
    public class SubscriptionSpecification
    {
        [Subject(typeof(Subscription))]
        public class in_general : with_subscription
        {
            It should_not_be_started = () =>
                Subject.IsStarted.ShouldBeFalse();
        }

        [Subject(typeof(Subscription))]
        public class when_getting_subscription_state : with_subscription
        {
            Because of = () =>
                state = Subject.State;

            It should_get_state = () =>
                state.ShouldNotBeNull();

            It should_query_delivery_strategy_for_state = () =>
                deliveryStrategy.WasToldTo(s => s.GetWorkerStates());

            static SubscriptionState state;
        }

        [Subject(typeof(Subscription))]
        public class when_checking_is_startable : with_subscription
        {
            Establish context = () =>
                deliveryStrategy.WhenToldTo(s => s.IsStartable).Return(true);

            Because of = () =>
                startable = Subject.IsStartable;

            It should_be_startable = () =>
                startable.ShouldBeTrue();

            static bool startable;
        }

        [Subject(typeof(Subscription))]
        public class when_starting : with_subscription
        {
            Establish context = () =>
                modelReference.WhenToldTo(r => r.GetConsumerQueue(Param.IsAny<IQueue>()))
                    .Return(new BlockingCollection<BasicDeliverEventArgs>());

            Because of = () =>
                Subject.Start();

            It should_be_started = () =>
                Subject.IsStarted.ShouldBeTrue();

            It should_start_delivery_strategy = () =>
                deliveryStrategy.WasToldTo(d => d.Start());
        }

        [Subject(typeof(Subscription))]
        public class when_disposing : WithSubject<Subscription>
        {
            Because of = () =>
                Subject.Dispose();

            It should_dispose_delivery_strategy = () =>
                The<IDeliveryStrategy>().WasToldTo(d => d.Stop());
        }

        public class with_subscription : WithFakes
        {
            Establish context = () =>
            {
                modelReference = An<IModelReference>();
                deliveryStrategy = An<IDeliveryStrategy>();
                deliveryQueue = An<IDeliveryQueue>();

                Subject = new Subscription(
                    modelReference,
                    deliveryStrategy,
                    new[] { deliveryQueue });
            };

            protected static Subscription Subject;

            protected static IModelReference modelReference;

            protected static IDeliveryStrategy deliveryStrategy;

            protected static IDeliveryQueue deliveryQueue;
        }
    }
}
