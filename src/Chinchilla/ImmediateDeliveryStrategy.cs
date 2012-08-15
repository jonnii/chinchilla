namespace Chinchilla
{
    public class ImmediateDeliveryStrategy : DeliveryStrategy
    {
        public override void Deliver(IDelivery delivery)
        {
            connectedProcessor.Process(delivery);
            delivery.Accept();
        }
    }
}