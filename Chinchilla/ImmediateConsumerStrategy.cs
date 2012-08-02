namespace Chinchilla
{
    public class ImmediateConsumerStrategy : IConsumerStrategy
    {
        private readonly IDeliveryHandler deliveryHandler;

        public ImmediateConsumerStrategy(IDeliveryHandler deliveryHandler)
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