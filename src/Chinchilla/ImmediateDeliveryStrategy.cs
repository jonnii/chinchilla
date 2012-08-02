namespace Chinchilla
{
    public class ImmediateDeliveryStrategy : IDeliveryStrategy
    {
        private readonly IDeliveryHandler deliveryHandler;

        public ImmediateDeliveryStrategy(IDeliveryHandler deliveryHandler)
        {
            this.deliveryHandler = deliveryHandler;
        }

        public void Deliver(IDelivery delivery)
        {
            deliveryHandler.Handle(delivery);
            delivery.Accept();
        }
    }
}