namespace Chinchilla
{
    public abstract class DeliveryStrategy : IDeliveryStrategy
    {
        private IDeliveryProcessor connectedProcessor;

        public void ConnectTo(IDeliveryProcessor processor)
        {
            connectedProcessor = processor;
        }

        public abstract void Start();

        public abstract void Deliver(IDelivery delivery);

        public abstract void Dispose();
    }
}