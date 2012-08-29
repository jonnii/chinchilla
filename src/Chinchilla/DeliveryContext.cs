namespace Chinchilla
{
    public class DeliveryContext : IDeliveryContext
    {
        public DeliveryContext(IBus bus)
        {
            Bus = bus;
        }

        public IBus Bus { get; private set; }
    }
}