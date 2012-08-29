using System;
using Chinchilla.Specifications.Messages;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class ConsumerSubscriberSpecification
    {
        [Subject(typeof(ConsumerSubscriber))]
        public class when_connecting_to_iconsumer : WithFakes
        {
            Establish context = () =>
                subscriber = new ConsumerSubscriber(An<IBus>(), An<IConsumer>());

            Because of = () =>
                exception = Catch.Exception(() => subscriber.Connect());

            It should_throw = () =>
                exception.ShouldBeOfType<ChinchillaException>();

            static ConsumerSubscriber subscriber;

            private static Exception exception;
        }

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
                bus.WasToldTo(b => b.Subscribe(Param.IsAny<Action<TestMessage, IDeliveryContext>>()));

            static IBus bus;

            static ConsumerSubscriber subscriber;
        }

        public class TestConsumer : IConsumer<TestMessage>
        {
            public void Consume(TestMessage message, IDeliveryContext deliveryContext)
            {

            }
        }
    }
}
