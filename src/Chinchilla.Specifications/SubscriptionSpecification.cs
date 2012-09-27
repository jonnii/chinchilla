using System;
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
        public class when_starting : with_subscription
        {
            Establish context = () =>
                modelReference.WhenToldTo(r => r.GetConsumerQueue(Param.IsAny<IQueue>()))
                    .Return(new BlockingCollection<BasicDeliverEventArgs>());

            Because of = () =>
                Subject.Start();

            It should_start_delivery_strategy = () =>
                deliveryStrategy.WasToldTo(d => d.Start());
        }

        [Subject(typeof(Subscription))]
        public class when_failed : WithSubject<Subscription>
        {
            Establish context = () => { };

            Because of = () =>
                Subject.OnFailed(An<IDelivery>(), new Exception());

            private It should_ = () => { };
        }

        [Subject(typeof(Subscription))]
        public class when_disposing : WithSubject<Subscription>
        {
            Because of = () =>
                Subject.Dispose();

            It should_dispose_delivery_strategy = () =>
                The<IDeliveryStrategy>().WasToldTo(d => d.Dispose());
        }

        public class with_subscription : WithFakes
        {
            Establish context = () =>
            {
                modelReference = An<IModelReference>();
                deliveryStrategy = An<IDeliveryStrategy>();
                faultStrategy = An<IFaultStrategy>();

                Subject = new Subscription(
                    modelReference,
                    deliveryStrategy,
                    faultStrategy,
                    new[] { new Queue("subscription-queue-name") });
            };

            protected static Subscription Subject;

            protected static IModelReference modelReference;

            protected static IDeliveryStrategy deliveryStrategy;

            protected static IFaultStrategy faultStrategy;
        }
    }
}
