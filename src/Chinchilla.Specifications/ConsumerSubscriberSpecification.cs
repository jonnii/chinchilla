using System;
using Chinchilla.Specifications.Messages;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class ConsumerSubscriberSpecification
    {
        [Subject(typeof(ConsumerSubscriber))]
        public class when_connecting : WithFakes
        {
            Establish context = () =>
            {
                bus = An<IBus>();
                subscriber = new ConsumerSubscriber(bus, new TestConsumer());
            };

            Because of = () =>
                subscriber.Connect();

            It should_subscribe = () =>
                bus.WasToldTo(b => b.Subscribe(Param.IsAny<Action<TestMessage>>()));

            static IBus bus;

            static ConsumerSubscriber subscriber;
        }

        [Subject(typeof(ConsumerSubscriber))]
        public class when_connecting_with_builder : WithFakes
        {
            Establish context = () =>
            {
                bus = An<IBus>();
                subscriber = new ConsumerSubscriber(bus, new TestConsumer());
            };

            Because of = () =>
                subscriber.Connect(_ => { });

            It should_subscribe_with_builders = () =>
                bus.WasToldTo(b => b.Subscribe(Param.IsAny<Action<TestMessage>>(), Param.IsAny<Action<ISubscriptionBuilder>>()));

            static IBus bus;

            static ConsumerSubscriber subscriber;
        }

        public class TestConsumer : IConsumer<TestMessage>
        {
            public void Consume(TestMessage message)
            {

            }
        }
    }
}
