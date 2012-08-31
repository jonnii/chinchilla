namespace Chinchilla.Configuration
{
    public interface ISubscriptionConfiguration : IEndpointConfiguration
    {
        string QueueName { get; }

        uint PrefetchSize { get; }

        ushort PrefetchCount { get; }

        IDeliveryStrategy BuildDeliveryStrategy(IDeliveryProcessor deliveryProcessor);

        IFaultStrategy BuildFaultStrategy(IBus bus);
    }
}