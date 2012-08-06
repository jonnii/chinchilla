namespace Chinchilla
{
    public class ImmediateDeliveryStrategy : IDeliveryStrategy
    {
        private IDeliveryHandler deliveryHandler;

        public void ConnectTo(IDeliveryHandler handler)
        {
            deliveryHandler = handler;
        }

        public void Start()
        {
        }

        public void Deliver(IDelivery delivery)
        {
            deliveryHandler.Handle(delivery);
            delivery.Accept();
        }

        public void Dispose()
        {

        }
    }
}