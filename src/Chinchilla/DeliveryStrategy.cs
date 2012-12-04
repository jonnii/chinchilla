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

        public virtual void Start()
        {

        }

        public abstract void Deliver(IDelivery delivery);

        public virtual void Stop()
        {

        }
    }
}