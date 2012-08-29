using System;
using Chinchilla.Topologies;

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

        /// <summary>
        /// Changes the strategy for failed deliveries
        /// </summary>
        /// <typeparam name="TStrategy">The type of the failure strategy</typeparam>
        /// <param name="configurations">Any configuration for this strategy</param>
        ISubscriptionBuilder DeliverFailuresUsing<TStrategy>(params Action<TStrategy>[] configurations)
            where TStrategy : IDeliveryFailureStrategy, new();

        ISubscriptionBuilder SetTopology(IMessageTopologyBuilder messageTopologyBuilder);

        ISubscriptionBuilder SetTopology<TBuilder>()
            where TBuilder : IMessageTopologyBuilder, new();

        ISubscriptionBuilder SubscribeOn(string subscriptionQueueName);
    }
}