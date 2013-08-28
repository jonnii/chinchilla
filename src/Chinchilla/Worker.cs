using System;

namespace Chinchilla
{
    public abstract class Worker : IWorker
    {
        private readonly IDeliveryProcessor connectedProcessor;

        protected Worker(IDeliveryProcessor connectedProcessor)
        {
            this.connectedProcessor = connectedProcessor;

            Status = WorkerStatus.Stopped;
        }

        public abstract string Name { get; }

        public abstract string Type { get; }

        public WorkerStatus Status { get; set; }

        public DateTime? BusySince { get; set; }

        public WorkerState GetState()
        {
            return new WorkerState(
                Name,
                Type,
                Status,
                BusySince);
        }

        public void Deliver(IDelivery delivery)
        {
            BeforeDeliver();

            try
            {
                connectedProcessor.Process(delivery);
                delivery.Accept();
            }
            catch (MessageRejectedException e)
            {
                delivery.Reject(e.ShouldRequeue);
            }
            catch (Exception e)
            {
                delivery.Failed(e);
            }

            AfterDeliver();
        }

        public void BeforeDeliver()
        {
            Status = WorkerStatus.Busy;
            BusySince = DateTime.UtcNow;
        }

        public void AfterDeliver()
        {
            Status = WorkerStatus.Idle;
            BusySince = null;
        }
    }
}