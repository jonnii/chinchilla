using System;
using System.Threading.Tasks;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class TaskDeliveryStrategySpecification
    {
        [Subject(typeof(TaskDeliveryStrategy))]
        public class when_handling_delivery : WithSubject<TaskDeliveryStrategy>
        {
            Establish context = () =>
            {
                processor = An<IDeliveryProcessor>();
                delivery = An<IDelivery>();

                Subject.ConnectTo(processor);
            };

            Because of = () =>
                Subject.DeliverOnTask(delivery).Wait();

            It should_process_delivery = () =>
                processor.WasToldTo(p => p.Process(Param.IsAny<IDelivery>()));

            It should_accept_delivery = () =>
                delivery.WasToldTo(d => d.Accept());

            static IDelivery delivery;

            static Task task;

            static IDeliveryProcessor processor;
        }

        [Subject(typeof(TaskDeliveryStrategy))]
        public class when_handling_delivery_that_throws_exception : WithSubject<TaskDeliveryStrategy>
        {
            Establish context = () =>
            {
                var processor = An<IDeliveryProcessor>();
                processor.WhenToldTo(p => p.Process(Param.IsAny<IDelivery>())).Throw(new Exception());

                delivery = An<IDelivery>();

                Subject.ConnectTo(processor);
            };

            Because of = () =>
                Subject.DeliverOnTask(delivery).Wait();

            It should_not_accept_delivery = () =>
                delivery.WasNotToldTo(d => d.Accept());

            It should_fail_delivery = () =>
                delivery.WasToldTo(d => d.Failed(Param.IsAny<Exception>()));

            static IDelivery delivery;

            static Task task;
        }
    }
}
