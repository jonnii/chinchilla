namespace Chinchilla
{
    public interface ISubscriptionConfiguration : IEndpointConfiguration
    {
        string QueueName { get; }

        IDeliveryStrategy BuildDeliveryStrategy(IDeliveryProcessor deliveryProcessor);

        IDeliveryFailureStrategy BuildDeliveryFailureStrategy();
    }
}