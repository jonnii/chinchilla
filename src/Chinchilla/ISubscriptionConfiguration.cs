using System;

namespace Chinchilla
{
    public interface ISubscriptionConfiguration
    {
        /// <summary>
        /// Changes the delivery strategy for the subscription.
        /// </summary>
        /// <typeparam name="TStrategy">The type of strategy to use</typeparam>
        /// <param name="configurations">Any configuration for this strategy</param>
        void DeliverUsing<TStrategy>(params Action<TStrategy>[] configurations)
            where TStrategy : IDeliveryStrategy, new();

        IDeliveryStrategy BuildDeliveryStrategy(IDeliveryProcessor deliveryProcessor);
    }
}