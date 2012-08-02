namespace Chinchilla
{
    public interface IConsumerStrategy
    {
        void Deliver(IDelivery delivery);
    }
}