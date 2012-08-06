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
                processor = An<IDeliveryProcessor>();
                delivery = An<IDelivery>();
                
                Subject.ConnectTo(processor);
            };

            Because of = () =>
                Subject.DeliverOne(delivery);

            It should_send_delivery_to_handler = () =>
                processor.WasToldTo(h => h.Process(Param.IsAny<IDelivery>()));

            It should_accept_delivery = () =>
                delivery.WasToldTo(d => d.Accept());

            static IDeliveryProcessor processor;

            static IDelivery delivery;
        }
    }
}
