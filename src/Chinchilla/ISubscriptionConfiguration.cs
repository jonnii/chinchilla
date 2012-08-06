using System;

namespace Chinchilla
{
    public interface ISubscriptionConfiguration
    {
        void DeliverUsing<TStrategy>(params Action<TStrategy>[] configurations)
            where TStrategy : IDeliveryStrategy, new();

        IDeliveryStrategy BuildDeliveryStrategy(IDeliveryHandler deliveryHandler);
    }
}