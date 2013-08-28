using System;
using System.Collections.Concurrent;
using System.Threading;
using Chinchilla.Threading;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class WorkerPoolWorkerSpecification
    {
        [Subject(typeof(WorkerPoolWorker))]
        public class in_general : with_worker_pool_thread
        {
            It should_have_name = () =>
                Subject.Name.ShouldEqual("pool-worker-1");
        }

        [Subject(typeof(WorkerPoolWorker))]
        public class when_starting : with_worker_pool_thread
        {
            Because of = () =>
                Subject.Start();

            It should_be_starting = () =>
                Subject.Status.ShouldEqual(WorkerStatus.Starting);

            It should_start_thread = () =>
                thread.WasToldTo(t => t.Start());
        }

        [Subject(typeof(WorkerPoolWorker))]
        public class when_calling_start_twice : with_worker_pool_thread
        {
            Establish context = () =>
                Subject.Start();

            Because of = () =>
                exception = Catch.Exception(() => Subject.Start());

            It should_throw_invalid_operation_exception = () =>
                exception.ShouldBeOfType<InvalidOperationException>();

            static Exception exception;
        }

        [Subject(typeof(WorkerPoolWorker))]
        public class when_stopping : with_worker_pool_thread
        {
            Establish context = () =>
                Subject.Start();

            Because of = () =>
                Subject.Stop();

            It should_be_in_stopped_state = () =>
                Subject.Status.ShouldEqual(WorkerStatus.Stopping);
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

        [Subject(typeof(WorkerPoolWorker))]
        public class when_delivering_one_message_that_throws_rejection_exception : with_worker_pool_thread
        {
            Establish context = () =>
            {
                processor.WhenToldTo(p => p.Process(Param.IsAny<IDelivery>())).Throw(new MessageRejectedException());
                delivery = An<IDelivery>();
            };

            Because of = () =>
                Subject.Deliver(delivery);

            It should_reject_message = () =>
                delivery.WasToldTo(d => d.Reject(true));

            static IDelivery delivery;
        }

        [Subject(typeof(WorkerPoolWorker))]
        public class when_pausing : with_worker_pool_thread
        {
            Because of = () =>
                Subject.Pause();

            It should_should_be_in_paused_state = () =>
                Subject.Status.ShouldEqual(WorkerStatus.Paused);
        }

        [Subject(typeof(WorkerPoolWorker))]
        public class when_resuming : with_worker_pool_thread
        {
            Establish context = () =>
                Subject.Pause();

            Because of = () =>
                Subject.Resume();

            It should_should_be_in_paused_state = () =>
                Subject.Status.ShouldEqual(WorkerStatus.Idle);
        }

        [Subject(typeof(WorkerPoolWorker))]
        public class when_resuming_unpaused_worker : with_worker_pool_thread
        {
            Because of = () =>
                exception = Catch.Exception(() => Subject.Resume());

            It should_throw_exception = () =>
                exception.ShouldBeOfType<InvalidOperationException>();

            static Exception exception;
        }

        public class with_worker_pool_thread : WithFakes
        {
            Establish context = () =>
            {
                var collection = new BlockingCollection<IDelivery>(new ConcurrentQueue<IDelivery>());
                processor = An<IDeliveryProcessor>();

                thread = An<IThread>();
                threadFactory = An<IThreadFactory>();
                threadFactory.WhenToldTo(t => t.Create(Param.IsAny<ThreadStart>()))
                    .Return(thread);

                Subject = new WorkerPoolWorker(1, threadFactory, collection, processor);
            };

            protected static IDeliveryProcessor processor;

            protected static WorkerPoolWorker Subject;

            protected static IThreadFactory threadFactory;

            protected static IThread thread;
        }
    }
}
