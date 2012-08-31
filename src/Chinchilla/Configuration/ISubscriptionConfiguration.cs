namespace Chinchilla.Configuration
{
    public interface ISubscriptionConfiguration : IEndpointConfiguration
    {
        string QueueName { get; }

        IDeliveryStrategy BuildDeliveryStrategy(IDeliveryProcessor deliveryProcessor);

        IFaultStrategy BuildDeliveryFailureStrategy(IBus bus);
    }
}