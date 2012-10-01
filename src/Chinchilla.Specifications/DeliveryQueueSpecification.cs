using System;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class DeliveryQueueSpecification
    {
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
