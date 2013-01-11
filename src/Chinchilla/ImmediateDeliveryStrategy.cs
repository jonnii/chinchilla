using System;

namespace Chinchilla
{
    public class ImmediateDeliveryStrategy : DeliveryStrategy
    {
        public override void Deliver(IDelivery delivery)
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

        public override WorkerState[] GetWorkerStates()
        {
            return new WorkerState[0];
        }
    }
}