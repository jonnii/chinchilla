using System;
using Chinchilla.Configuration;
using Chinchilla.Specifications.Messages;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class BusSpecification
    {
        [Subject(typeof(Bus))]
        public class when_creating_publisher : with_bus
        {
            Because of = () =>
                Subject.CreatePublisher<TestMessage>();

            It should_create_new_model = () =>
                The<IModelFactory>().WasToldTo(c => c.CreateModel());

            It should_create_publisher = () =>
                The<IPublisherFactory>().WasToldTo(f => f.Create<TestMessage>(
                    Param.IsAny<IModelReference>(),
                    Param.IsAny<IPublisherConfiguration>()));

            static IPublisher<TestMessage> publisher;
        }

        [Subject(typeof(Bus))]
        public class when_publishing : with_bus
        {
            Establish context = () =>
            {
                publisher = An<IPublisher<TestMessage>>();

                The<IPublisherFactory>().WhenToldTo(f => f.Create<TestMessage>(
                    Param.IsAny<IModelReference>(),
                    Param.IsAny<IPublisherConfiguration>())).Return(publisher);
            };

            Because of = () =>
                Subject.Publish(new TestMessage());

            It should_create_new_model = () =>
                The<IModelFactory>().WasToldTo(c => c.CreateModel());

            It should_publish_the_message = () =>
                publisher.WasToldTo(p => p.Publish(Param.IsAny<TestMessage>()));

            static IPublisher<TestMessage> publisher;
        }

        [Subject(typeof(Bus))]
        public class when_subscribing : with_subscription
        {
            Because of = () =>
                Subject.Subscribe<TestMessage>(_ => { });

            It should_start_subscription = () =>
                subscription.WasToldTo(s => s.Start());
        }

        [Subject(typeof(Bus))]
        public class when_subscribing_with_invalid_subscription_configuration : with_subscription
        {
            Establish context = () =>
                subscription.WhenToldTo(s => s.IsStartable).Return(false);

            Because of = () =>
                Subject.Subscribe<TestMessage>(_ => { });

            It should_not_start_subscription = () =>
                subscription.WasNotToldTo(s => s.Start());
        }

        [Subject(typeof(Bus))]
        public class when_subscribing_to_consumer_instance : with_subscription
        {
            Because of = () =>
                Subject.Subscribe(new TestConsumer());

            It should_start_subscription = () =>
                subscription.WasToldTo(s => s.Start());
        }

        [Subject(typeof(Bus))]
        public class when_subscribing_to_consumer_type : with_subscription
        {
            Establish context = () =>
                The<IConsumerFactory>().WhenToldTo(f => f.Build<TestConsumer>())
                    .Return(new TestConsumer());

            Because of = () =>
                Subject.Subscribe<TestConsumer>();

            It should_create_subscription = () =>
                The<IConsumerFactory>().WasToldTo(f => f.Build<TestConsumer>());
        }

        [Subject(typeof(Bus))]
        public class when_disposing : with_bus
        {
            Because of = () =>
                Subject.Dispose();

            It should_dispose_of_subscription_factory = () =>
                The<ISubscriptionFactory>().WasToldTo(f => f.Dispose());

            It should_dispose_model_factory = () =>
                The<IModelFactory>().WasToldTo(f => f.Dispose());

            It should_dispose_consumer_factory = () =>
                The<IConsumerFactory>().WasToldTo(f => f.Dispose());

            It should_dispose_of_publisher_factory = () =>
                The<IPublisherFactory>().WasToldTo(f => f.Dispose());
        }

        public class with_bus : WithSubject<Bus>
        {
            Establish context = () =>
                The<IModelFactory>().WhenToldTo(c => c.CreateModel()).Return(An<IModelReference>());
        }

        public class with_subscription : with_bus
        {
            Establish establish = () =>
            {
                subscription = An<ISubscription>();
                subscription.WhenToldTo(s => s.IsStartable).Return(true);

                The<ISubscriptionFactory>().WhenToldTo(
                    s => s.Create(
                        Param.IsAny<IBus>(),
                        Param.IsAny<ISubscriptionConfiguration>(), Param.IsAny<Action<TestMessage, IDeliveryContext>>())).Return(subscription);
            };

            protected static ISubscription subscription;
        }

        public class TestConsumer : IConsumer<TestMessage>
        {
            public void Consume(TestMessage message, IDeliveryContext deliveryContext)
            {

            }
        }
    }
}
