using System;
using System.Collections.Concurrent;
using Chinchilla.Threading;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class WorkerPoolWorkerSpecification
    {
        [Subject(typeof(WorkerPoolWorker))]
        public class when_calling_start_twice
        {
            Establish context = () => { };

            private Because of = () => { };

            private It should_ = () => { };
        }

        [Subject(typeof(WorkerPoolWorker))]
        public class when_getting_state_before_starting : with_worker_pool_thread
        {
            Because of = () =>
                state = Subject.GetState();

            It should_be_stopped = () =>
                state.Status.ShouldEqual(WorkerStatus.Stopped);

            static WorkerState state;
        }

        [Subject(typeof(WorkerPoolWorker))]
        public class before_message_pump_started : with_worker_pool_thread
        {
            Because of = () =>
            {
                Subject.BeforeStartMessagePump();
                state = Subject.GetState();
            };

            It should_be_idle = () =>
                state.Status.ShouldEqual(WorkerStatus.Idle);

            static WorkerState state;
        }

        [Subject(typeof(WorkerPoolWorker))]
        public class before_delivery : with_worker_pool_thread
        {
            Because of = () =>
            {
                Subject.BeforeDeliver();
                state = Subject.GetState();
            };

            It should_be_busy = () =>
                state.Status.ShouldEqual(WorkerStatus.Busy);

            It should_set_busy_since = () =>
                state.BusySince.Value.ShouldBeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));

            static WorkerState state;
        }

        [Subject(typeof(WorkerPoolWorker))]
        public class after_delivery : with_worker_pool_thread
        {
            Because of = () =>
            {
                Subject.AfterDeliver();
                state = Subject.GetState();
            };

            It should_be_idle = () =>
                state.Status.ShouldEqual(WorkerStatus.Idle);

            It should_not_be_busy_since = () =>
                state.BusySince.ShouldBeNull();

            static WorkerState state;
        }

        [Subject(typeof(WorkerPoolWorker))]
        public class when_delivering_one_message : with_worker_pool_thread
        {
            Establish context = () =>
                delivery = An<IDelivery>();

            Because of = () =>
                Subject.Deliver(delivery);

            It should_send_delivery_to_handler = () =>
                processor.WasToldTo(h => h.Process(Param.IsAny<IDelivery>()));

            It should_accept_delivery = () =>
                delivery.WasToldTo(d => d.Accept());

            static IDelivery delivery;
        }

        [Subject(typeof(WorkerPoolWorker))]
        public class when_delivering_one_message_that_throws_exception : with_worker_pool_thread
        {
            Establish context = () =>
            {
                processor.WhenToldTo(p => p.Process(Param.IsAny<IDelivery>())).Throw(new Exception());
                delivery = An<IDelivery>();
            };

            Because of = () =>
                Subject.Deliver(delivery);

            It should_not_accept_delivery = () =>
                delivery.WasNotToldTo(d => d.Accept());

            It should_fail_delivery = () =>
                delivery.WasToldTo(d => d.Failed(Param.IsAny<Exception>()));

            static IDelivery delivery;
        }

        public class with_worker_pool_thread : WithFakes
        {
            Establish context = () =>
            {
                var collection = new BlockingCollection<IDelivery>(new ConcurrentQueue<IDelivery>());
                processor = An<IDeliveryProcessor>();

                threadFactory = An<IThreadFactory>();

                Subject = new WorkerPoolWorker(threadFactory, collection, processor);
            };

            protected static IDeliveryProcessor processor;

            protected static WorkerPoolWorker Subject;

            protected static IThreadFactory threadFactory;
        }
    }
}
