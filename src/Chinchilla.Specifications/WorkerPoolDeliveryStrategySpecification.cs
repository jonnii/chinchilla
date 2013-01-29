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
                states = Subject.GetWorkerStates();

            It should_get_state = () =>
                states.ShouldNotBeNull();

            static WorkerState[] states;
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
        public class when_getting_workers_controller_when_stopped : WithSubject<WorkerPoolDeliveryStrategy>
        {
            Because of = () =>
                exception = Catch.Exception(() => Subject.GetWorkersController());

            It should_be_pool_delivery_workers_controller = () =>
                exception.ShouldBeOfType<ChinchillaException>();

            static Exception exception;
        }

        [Subject(typeof(WorkerPoolDeliveryStrategy))]
        public class when_getting_workers_controller : WithSubject<WorkerPoolDeliveryStrategy>
        {
            Establish context = () =>
                Subject.Start();

            Because of = () =>
                controller = Subject.GetWorkersController();

            It should_be_pool_delivery_workers_controller = () =>
                controller.ShouldBeOfType<WorkerPoolWorkersController>();

            static IWorkersController controller;
        }
    }
}
