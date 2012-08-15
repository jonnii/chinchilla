using System;

namespace Chinchilla
{
    public interface ISubscriptionBuilder
    {
        /// <summary>
        /// Changes the delivery strategy for the subscription.
        /// </summary>
        /// <typeparam name="TStrategy">The type of strategy to use</typeparam>
        /// <param name="configurations">Any configuration for this strategy</param>
        ISubscriptionBuilder DeliverUsing<TStrategy>(params Action<TStrategy>[] configurations)
            where TStrategy : IDeliveryStrategy, new();
    }
}