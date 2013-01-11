using System;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class ImmediateDeliveryStrategySpecification
    {
        [Subject(typeof(ImmediateDeliveryStrategy))]
        public class when_querying_for_state : WithSubject<ImmediateDeliveryStrategy>
        {
            Because of = () =>
                states = Subject.GetWorkerStates();

            It should_get_state = () =>
                states.ShouldNotBeNull();

            It should_have_single_idle_worker = () =>
                states.Single().Status.ShouldEqual(WorkerStatus.Stopped);

            static WorkerState[] states;
        }

        [Subject(typeof(ImmediateDeliveryStrategy))]
        public class when_started : WithSubject<ImmediateDeliveryStrategy>
        {
            Establish context = () =>
                Subject.Start();

            Because of = () =>
                states = Subject.GetWorkerStates();

            It should_have_single_idle_worker = () =>
                states.Single().Status.ShouldEqual(WorkerStatus.Idle);

            static WorkerState[] states;
        }

        [Subject(typeof(ImmediateDeliveryStrategy))]
        public class when_started_then_stopped : WithSubject<ImmediateDeliveryStrategy>
        {
            Establish context = () =>
            {
                Subject.Start();
                Subject.Stop();
            };

            Because of = () =>
                states = Subject.GetWorkerStates();

            It should_have_single_idle_worker = () =>
                states.Single().Status.ShouldEqual(WorkerStatus.Stopped);

            static WorkerState[] states;
        }

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
