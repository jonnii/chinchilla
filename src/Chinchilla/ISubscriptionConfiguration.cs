using Chinchilla.Topologies;

namespace Chinchilla
{
    public interface ISubscriptionConfiguration
    {
        IDeliveryStrategy BuildDeliveryStrategy(IDeliveryProcessor deliveryProcessor);

        ISubscriptionTopology BuildTopology(string messageType);
    }
}