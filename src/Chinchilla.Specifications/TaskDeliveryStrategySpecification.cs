﻿using System;
using System.Threading.Tasks;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class TaskDeliveryStrategySpecification
    {
        [Subject(typeof(TaskDeliveryStrategy))]
        public class in_general : WithSubject<TaskDeliveryStrategy>
        {
            It should_have_no_workers = () =>
                Subject.NumWorkers.ShouldEqual(0);
        }

        [Subject(typeof(TaskDeliveryStrategy))]
        public class when_querying_for_state : WithSubject<TaskDeliveryStrategy>
        {
            Because of = () =>
                states = Subject.GetWorkerStates();

            It should_get_state = () =>
                states.ShouldNotBeNull();

            static WorkerState[] states;
        }

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
        }

        [Subject(typeof(TaskDeliveryStrategy))]
        public class when_delivering_tasks : WithSubject<TaskDeliveryStrategy>
        {
            Because of = () =>
            {
                var deliveries = new[]
                {
                    Subject.DeliverOnTask(An<IDelivery>()),
                    Subject.DeliverOnTask(An<IDelivery>()),
                    Subject.DeliverOnTask(An<IDelivery>()),
                    Subject.DeliverOnTask(An<IDelivery>())
                };

                Task.WaitAll(deliveries);
            };

            It should_have_no_workers = () =>
                Subject.NumWorkers.ShouldEqual(0);
        }
    }
}
