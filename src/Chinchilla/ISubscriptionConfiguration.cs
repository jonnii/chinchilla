using Chinchilla.Topologies;

namespace Chinchilla
{
    public interface ISubscriptionConfiguration
    {
        IDeliveryStrategy BuildDeliveryStrategy(IDeliveryProcessor deliveryProcessor);

        ISubscriberTopology BuildTopology(string messageType);
    }
}