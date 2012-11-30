using System;
using Machine.Fakes;
using Machine.Specifications;
using RabbitMQ.Client.Events;

namespace Chinchilla.Specifications
{
    public class DeliveryQueueSpecification
    {
        [Subject(typeof(DeliveryQueue))]
        public class when_taking_message_before_starting : WithSubject<DeliveryQueue>
        {
            Because of = () =>
            {
                BasicDeliverEventArgs e;
                exception = Catch.Exception(() => Subject.TryTake(out e));
            };

            It should_ = () =>
                exception.ShouldBeOfType<InvalidOperationException>();

            static Exception exception;
        }

        [Subject(typeof(DeliveryQueue))]
        public class on_failed : WithSubject<DeliveryQueue>
        {
            Because of = () =>
                Subject.OnFailed(An<IDelivery>(), new Exception());

            It should_send_failed_delivery_to_fault_strategy = () =>
                The<IFaultStrategy>().WasToldTo(
                    s => s.ProcessFailedDelivery(
                        Param.IsAny<IDelivery>(),
                        Param.IsAny<Exception>()));
        }
    }
}
