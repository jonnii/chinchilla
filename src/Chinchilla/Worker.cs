using System;

namespace Chinchilla
{
    public abstract class Worker
    {
        private readonly IDeliveryProcessor connectedProcessor;

        protected Worker(IDeliveryProcessor connectedProcessor)
        {
            this.connectedProcessor = connectedProcessor;

            Status = WorkerStatus.Stopped;
        }

        public abstract string WorkerType { get; }

        public WorkerStatus Status { get; set; }

        public DateTime? BusySince { get; set; }

        public WorkerState GetState()
        {
            return new WorkerState(WorkerType, Status, BusySince);
        }

        public void BeforeDeliver()
        {
            Status = WorkerStatus.Busy;
            BusySince = DateTime.UtcNow;
        }

        public void Deliver(IDelivery delivery)
        {
            BeforeDeliver();

            try
            {
                connectedProcessor.Process(delivery);
                delivery.Accept();
            }
            catch (Exception e)
            {
                delivery.Failed(e);
            }

            AfterDeliver();
        }

        public void AfterDeliver()
        {
            Status = WorkerStatus.Idle;
            BusySince = null;
        }
    }
}