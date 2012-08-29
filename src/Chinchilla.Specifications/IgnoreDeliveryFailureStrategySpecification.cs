using System;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class IgnoreDeliveryFailureStrategySpecification
    {
        [Subject(typeof(IDeliveryFailureStrategy))]
        public class when_handling_delivery_exception : WithSubject<IgnoreDeliveryFailureStrategy>
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
