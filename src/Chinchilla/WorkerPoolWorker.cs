using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Chinchilla
{
    public class WorkerPoolWorker : Worker
    {
        private readonly BlockingCollection<IDelivery> deliveries;

        private readonly IDeliveryProcessor connectedProcessor;

        private readonly Thread thread;

        public WorkerPoolWorker(
            BlockingCollection<IDelivery> deliveries,
            IDeliveryProcessor connectedProcessor)
        {
            this.deliveries = deliveries;
            this.connectedProcessor = connectedProcessor;

            thread = new Thread(StartTakingMessages);
        }

        public override string WorkerType
        {
            get { return "WorkerPoolThread"; }
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

                using (StartWorkingScope())
                {
                    Deliver(delivery);
                }
            }
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
    }
}