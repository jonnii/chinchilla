using System;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class ErrorQueueDeliveryFailureStrategySpecification
    {
        [Subject(typeof(ErrorQueueDeliveryFailureStrategy))]
        public class when_handling_failed_delivery : WithSubject<ErrorQueueDeliveryFailureStrategy>
        {
            Establish context = () =>
                delivery = An<IDelivery>();

            Because of = () =>
                Subject.Handle(delivery, new Exception());

            It should_accept_delivery = () =>
                delivery.WasToldTo(d => d.Accept());

            static IDelivery delivery;
        }
    }
}
