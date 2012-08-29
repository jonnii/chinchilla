using System;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class ImmediateDeliveryStrategySpecification
    {
        [Subject(typeof(ImmediateDeliveryStrategy))]
        public class when_delivery_processor_throws_exception : WithSubject<ImmediateDeliveryStrategy>
        {
            Establish context = () =>
            {
                processor = An<IDeliveryProcessor>();
                processor.WhenToldTo(p => p.Process(Param.IsAny<IDelivery>())).Throw(new Exception());

                delivery = An<IDelivery>();

                Subject.ConnectTo(processor);
            };

            Because of = () =>
                Subject.Deliver(delivery);

            It should_call_failed_on_delivery = () =>
                delivery.WasToldTo(d => d.Failed(Param.IsAny<Exception>()));

            It should_not_accept_delivery = () =>
                delivery.WasNotToldTo(d => d.Accept());

            static IDeliveryProcessor processor;

            static IDelivery delivery;
        }
    }
}
