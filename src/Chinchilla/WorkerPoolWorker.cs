using System;
using System.Collections.Concurrent;
using Chinchilla.Threading;

namespace Chinchilla
{
    public class WorkerPoolWorker : Worker
    {
        private readonly BlockingCollection<IDelivery> deliveries;

        private readonly IThread thread;

        public WorkerPoolWorker(
            IThreadFactory threadFactory,
            BlockingCollection<IDelivery> deliveries,
            IDeliveryProcessor connectedProcessor)
            : base(connectedProcessor)
        {
            this.deliveries = deliveries;

            thread = threadFactory.Create(StartTakingMessages);
        }

        public override string WorkerType
        {
            get { return "WorkerPool"; }
        }

        public bool IsStopping { get; set; }

        public void Start()
        {
            if (Status != WorkerStatus.Stopped)
            {
                throw new InvalidOperationException(
                    "Cannot start this worker because it has already been started");
            }

            Status = WorkerStatus.Starting;
            thread.Start();
        }

        public void Join()
        {
            thread.Join();
        }

        public void StopAndJoin()
        {
            IsStopping = true;
            thread.Join();
        }

        public void BeforeStartMessagePump()
        {
            Status = WorkerStatus.Idle;
        }

        public void StartTakingMessages()
        {
            BeforeStartMessagePump();
            PumpMessages();
        }

        private void PumpMessages()
        {
            while (!IsStopping)
            {
                IDelivery delivery;

                try
                {
                    delivery = deliveries.Take();
                }
                catch (InvalidOperationException)
                {
                    break;
                }

                Deliver(delivery);
            }
        }
    }
}