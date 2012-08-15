using System;
using Chinchilla.Specifications.Messages;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class BusSpecification
    {
        [Subject(typeof(Bus))]
        public class in_general : with_bus
        {
            It should_have_topology = () =>
                Subject.Topology.ShouldNotBeNull();
        }

        [Subject(typeof(Bus))]
        public class when_creating_publisher : with_bus
        {
            Establish context = () =>
            {
                publisher = An<IPublisher<TestMessage>>();

                The<IPublisherFactory>().WhenToldTo(f => f.Create<TestMessage>(
                    Param.IsAny<IModelReference>(),
                    Param.IsAny<IPublisherConfiguration>())).Return(publisher);
            };

            Because of = () =>
                Subject.CreatePublisher<TestMessage>();

            It should_create_new_model = () =>
                The<IModelFactory>().WasToldTo(c => c.CreateModel());

            It should_create_publisher = () =>
                The<IPublisherFactory>().WasToldTo(f => f.Create<TestMessage>(
                    Param.IsAny<IModelReference>(),
                    Param.IsAny<IPublisherConfiguration>()));

            It should_start_publisher = () =>
                publisher.WasToldTo(p => p.Start());

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

            static IPublisher<TestMessage> publisher;
        }

        [Subject(typeof(Bus))]
        public class when_subscribing : with_bus
        {
            Establish establish = () =>
            {
                subscription = An<ISubscription>();
                The<ISubscriptionFactory>().WhenToldTo(
                    s => s.Create(
                        Param.IsAny<IModelReference>(),
                        Param.IsAny<ISubscriptionConfiguration>(), Param.IsAny<Action<TestMessage>>())).Return(subscription);
            };

            Because of = () =>
                Subject.Subscribe<TestMessage>(_ => { });

            It should_create_new_model = () =>
                The<IModelFactory>().WasToldTo(c => c.CreateModel());

            It should_start_subscription = () =>
                subscription.WasToldTo(s => s.Start());

            static ISubscription subscription;
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
        }

        public class with_bus : WithSubject<Bus>
        {
            Establish context = () =>
                The<IModelFactory>().WhenToldTo(c => c.CreateModel()).Return(An<IModelReference>());
        }
    }
}
