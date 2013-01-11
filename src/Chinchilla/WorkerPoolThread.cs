using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Chinchilla
{
    public class WorkerPoolThread
    {
        private readonly BlockingCollection<IDelivery> deliveries;

        private readonly IDeliveryProcessor connectedProcessor;

        private readonly Thread thread;

        private WorkerStatus status = WorkerStatus.Stopped;

        public WorkerPoolThread(
            BlockingCollection<IDelivery> deliveries,
            IDeliveryProcessor connectedProcessor)
        {
            this.deliveries = deliveries;
            this.connectedProcessor = connectedProcessor;

            thread = new Thread(StartTakingMessages);
        }

        public bool IsStopping { get; set; }

        public void Start()
        {
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

        public WorkerState GetState()
        {
            return new WorkerState("WorkerPoolThread", status);
        }

        public void BeforeStartMessagePump()
        {
            status = WorkerStatus.Idle;
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

                BeforeDeliver();
                Deliver(delivery);
                AfterDeliver();
            }
        }

        public void BeforeDeliver()
        {
            status = WorkerStatus.Busy;
        }

        public void Deliver(IDelivery delivery)
        {
            try
            {
                connectedProcessor.Process(delivery);
            }
            catch (Exception e)
            {
                delivery.Failed(e);
                return;
            }

            delivery.Accept();
        }

        public void AfterDeliver()
        {
            status = WorkerStatus.Idle;
        }
    }
}