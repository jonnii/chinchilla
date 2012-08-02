using System;

namespace Chinchilla
{
    public interface ISubscriptionConfiguration
    {
        void ConsumerStrategy<TStrategy>(params Action<TStrategy>[] configuration)
            where TStrategy : IDeliveryStrategy;

        IDeliveryStrategy BuildConsumerStrategy(IDeliveryHandler deliveryHandler);
    }
}