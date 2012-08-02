using System;

namespace Chinchilla
{
    public interface ISubscriptionConfiguration
    {
        void ConsumerStrategy<TStrategy>(params Action<TStrategy>[] configuration)
            where TStrategy : IConsumerStrategy;

        IConsumerStrategy BuildConsumerStrategy(IDeliveryHandler deliveryHandler);
    }
}