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
        public class when_starting : WithSubject<Subscription>
        {
            Establish context = () =>
                The<IModelReference>().WhenToldTo(r => r.GetConsumerQueue(Param.IsAny<IQueue>()))
                    .Return(new BlockingCollection<BasicDeliverEventArgs>());

            Because of = () =>
                Subject.Start();

            It should_start_delivery_strategy = () =>
                The<IDeliveryStrategy>().WasToldTo(d => d.Start());
        }

        [Subject(typeof(Subscription))]
        public class when_ : WithSubject<Subscription>
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
    }
}
