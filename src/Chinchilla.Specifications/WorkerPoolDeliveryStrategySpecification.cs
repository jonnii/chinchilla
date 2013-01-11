using System;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class WorkerPoolDeliveryStrategySpecification
    {
        [Subject(typeof(WorkerPoolDeliveryStrategy))]
        public class when_querying_for_state : WithSubject<WorkerPoolDeliveryStrategy>
        {
            Because of = () =>
                state = Subject.GetState();

            It should_get_state = () =>
                state.ShouldNotBeNull();

            static DeliveryStrategyState state;
        }

        [Subject(typeof(WorkerPoolDeliveryStrategy))]
        public class with_no_delivery_threads : WithSubject<WorkerPoolDeliveryStrategy>
        {
            Establish context = () =>
                Subject.NumWorkers = 0;

            It should_not_be_startable = () =>
                Subject.IsStartable.ShouldBeFalse();
        }

        [Subject(typeof(WorkerPoolDeliveryStrategy))]
        public class with_delivery_threads : WithSubject<WorkerPoolDeliveryStrategy>
        {
            Establish context = () =>
                Subject.NumWorkers = 4;

            It should_not_be_startable = () =>
                Subject.IsStartable.ShouldBeTrue();
        }

        [Subject(typeof(WorkerPoolDeliveryStrategy))]
        public class when_starting_with_no_delivery_threads : WithSubject<WorkerPoolDeliveryStrategy>
        {
            Establish context = () =>
               Subject.NumWorkers = 0;

            Because of = () =>
                exception = Catch.Exception(() => Subject.Start());

            It should_throw_exception = () =>
                exception.ShouldBeOfType<ChinchillaException>();

            static Exception exception;
        }

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

        [Subject(typeof(WorkerPoolDeliveryStrategy))]
        public class when_delivering_one_message_that_throws_exception : WithSubject<WorkerPoolDeliveryStrategy>
        {
            Establish context = () =>
            {
                var processor = An<IDeliveryProcessor>();
                processor.WhenToldTo(p => p.Process(Param.IsAny<IDelivery>())).Throw(new Exception());

                delivery = An<IDelivery>();

                Subject.ConnectTo(processor);
            };

            Because of = () =>
                Subject.DeliverOne(delivery);

            It should_not_accept_delivery = () =>
                delivery.WasNotToldTo(d => d.Accept());

            It should_fail_delivery = () =>
                delivery.WasToldTo(d => d.Failed(Param.IsAny<Exception>()));

            static IDelivery delivery;
        }
    }
}
