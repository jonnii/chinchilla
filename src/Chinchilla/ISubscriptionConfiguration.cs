using Chinchilla.Topologies;

namespace Chinchilla
{
    public interface ISubscriptionConfiguration
    {
        string QueueName { get; }

        IDeliveryStrategy BuildDeliveryStrategy(IDeliveryProcessor deliveryProcessor);

        IMessageTopology BuildTopology(IEndpoint endpoint);
    }
}