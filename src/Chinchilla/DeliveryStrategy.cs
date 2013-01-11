namespace Chinchilla
{
    public abstract class DeliveryStrategy : IDeliveryStrategy
    {
        protected IDeliveryProcessor connectedProcessor;

        public void ConnectTo(IDeliveryProcessor processor)
        {
            connectedProcessor = processor;
        }

        public virtual bool IsStartable
        {
            get { return true; }
        }

        public abstract WorkerState[] GetWorkerStates();

        public virtual void Start()
        {

        }

        public virtual void Stop()
        {

        }

        public abstract void Deliver(IDelivery delivery);
    }
}