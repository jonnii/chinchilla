using System;

namespace Chinchilla
{
    public class ImmediateDeliveryStrategy : DeliveryStrategy
    {
        private WorkerStatus status;

        public ImmediateDeliveryStrategy()
        {
            status = WorkerStatus.Stopped;
        }

        public override void Start()
        {
            status = WorkerStatus.Idle;
        }

        public override void Stop()
        {
            status = WorkerStatus.Stopped;
        }

        public override void Deliver(IDelivery delivery)
        {
            status = WorkerStatus.Busy;

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

            status = WorkerStatus.Idle;
        }

        public override WorkerState[] GetWorkerStates()
        {
            return new[]
            {
                new WorkerState("Immediate", status) 
            };
        }
    }
}