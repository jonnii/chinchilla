namespace Chinchilla
{
    public class ImmediateDeliveryStrategy : IDeliveryStrategy
    {
        private IDeliveryProcessor deliveryProcessor;

        public void ConnectTo(IDeliveryProcessor processor)
        {
            deliveryProcessor = processor;
        }

        public void Start()
        {
        }

        public void Deliver(IDelivery delivery)
        {
            deliveryProcessor.Process(delivery);
            delivery.Accept();
        }

        public void Dispose()
        {

        }
    }
}