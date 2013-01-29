using System;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class WorkerPoolWorkersControllerSpecification
    {
        [Subject(typeof(WorkerPoolWorkersController))]
        public class when_pausing_unknown_worker : with_controller
        {
            Because of = () =>
                exception = Catch.Exception(() => controller.Pause("fribble"));

            It should_throw = () =>
                exception.ShouldBeOfType<ChinchillaException>();

            static Exception exception;
        }

        [Subject(typeof(WorkerPoolWorkersController))]
        public class when_pausing_worker : with_controller
        {
            Because of = () =>
                controller.Pause("worker-1");

            It should_pause_worker = () =>
                worker1.WasToldTo(w => w.Pause());
        }

        [Subject(typeof(WorkerPoolWorkersController))]
        public class when_resuming_worker : with_controller
        {
            Because of = () =>
                controller.Resume("worker-2");

            It should_resume_worker = () =>
                worker2.WasToldTo(w => w.Resume());
        }

        public class with_controller : WithFakes
        {
            Establish context = () =>
            {
                worker1 = An<IWorkerPoolWorker>();
                worker1.WhenToldTo(w => w.Name).Return("worker-1");

                worker2 = An<IWorkerPoolWorker>();
                worker2.WhenToldTo(w => w.Name).Return("worker-2");

                controller = new WorkerPoolWorkersController(
                    new[] { worker1, worker2 });
            };

            protected static IWorkerPoolWorker worker1;

            protected static IWorkerPoolWorker worker2;

            protected static WorkerPoolWorkersController controller;
        }
    }
}
