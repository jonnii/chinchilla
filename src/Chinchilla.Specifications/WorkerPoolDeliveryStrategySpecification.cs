using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class WorkerPoolDeliveryStrategySpecification
    {
        [Subject(typeof(WorkerPoolDeliveryStrategy))]
        public class when_delivering_one_message : WithSubject<WorkerPoolDeliveryStrategy>
        {
            Establish context = () =>
            {
                handler = An<IDeliveryHandler>();
                delivery = An<IDelivery>();
                
                Subject.ConnectTo(handler);
            };

            Because of = () =>
                Subject.DeliverOne(delivery);

            It should_send_delivery_to_handler = () =>
                handler.WasToldTo(h => h.Handle(Param.IsAny<IDelivery>()));

            It should_accept_delivery = () =>
                delivery.WasToldTo(d => d.Accept());

            static IDeliveryHandler handler;

            static IDelivery delivery;
        }
    }
}
