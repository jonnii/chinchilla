using System;
using Chinchilla.Configuration;
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
                exception.ShouldBeAssignableTo<ChinchillaException>();

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

        [Subject(typeof(ConsumerSubscriber))]
        public class when_connecting_with_consumer_with_multiple_consumers : WithFakes
        {
            Establish context = () =>
            {
                bus = An<IBus>();
                subscriber = new ConsumerSubscriber(bus, new MultipleInterfaceConsumer());
            };

            Because of = () =>
                exception = Catch.Exception(() => subscriber.Connect());

            It should_create_multi_subscription = () =>
                exception.ShouldBeAssignableTo<ChinchillaException>();

            static IBus bus;

            static ConsumerSubscriber subscriber;

            static Exception exception;
        }

        [Subject(typeof(ConsumerSubscriber))]
        public class when_connecting_with_custom_configuration : WithFakes
        {
            Establish context = () =>
            {
                bus = An<IBus>();
                subscriber = new ConsumerSubscriber(bus, new TestConsumerWithConfiguration());
            };

            Because of = () =>
                subscriber.Connect();

            It should_subscribe_with_configuration_builder = () =>
                bus.WasToldTo(b => b.Subscribe(Param.IsAny<Action<TestMessage, IDeliveryContext>>(), Param.IsAny<Action<ISubscriptionBuilder>>()));

            static IBus bus;

            static ConsumerSubscriber subscriber;
        }

        public class TestConsumer : IConsumer<TestMessage>
        {
            public void Consume(TestMessage message, IDeliveryContext deliveryContext)
            {

            }
        }

        public class MultipleInterfaceConsumer : IConsumer<TestMessage>, IConsumer<AnotherTestMessage>
        {
            public void Consume(TestMessage message, IDeliveryContext deliveryContext)
            {

            }

            public void Consume(AnotherTestMessage message, IDeliveryContext deliveryContext)
            {

            }
        }

        public class TestConsumerWithConfiguration : IConsumer<TestMessage>, IConfigurableConsumer
        {
            public void Consume(TestMessage message, IDeliveryContext deliveryContext)
            {
            }

            public void ConfigureSubscription(ISubscriptionBuilder builder)
            {
            }
        }
    }
}
