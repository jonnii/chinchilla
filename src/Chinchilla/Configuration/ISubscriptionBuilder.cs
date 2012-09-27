using System;
using Chinchilla.Topologies;

namespace Chinchilla.Configuration
{
    public interface ISubscriptionBuilder
    {
        /// <summary>
        /// Changes the delivery strategy for the subscription.
        /// </summary>
        ISubscriptionBuilder DeliverUsing<TStrategy>(params Action<TStrategy>[] configurations)
            where TStrategy : IDeliveryStrategy, new();

        /// <summary>
        /// Changes the strategy for faulted deliveries
        /// </summary>
        ISubscriptionBuilder DeliverFaultsUsing<TStrategy>(params Action<TStrategy>[] configurations)
            where TStrategy : IFaultStrategy, new();

        /// <summary>
        /// Sets the topology of this subscription
        /// </summary>
        ISubscriptionBuilder SetTopology(IMessageTopologyBuilder builder);

        /// <summary>
        /// Sets the topology of this subscription
        /// </summary>
        ISubscriptionBuilder SetTopology<TBuilder>()
            where TBuilder : IMessageTopologyBuilder, new();

        /// <summary>
        /// Changes the queue name that this subscription subscribes on.
        /// </summary>
        /// <param name="endpointName">The name of the endpoint to subscribe on</param>
        /// <param name="otherEndpointNames">Any additional endpoints to subscribe on</param>
        ISubscriptionBuilder SubscribeOn(string endpointName, params string[] otherEndpointNames);

        /// <summary>
        /// Changes the prefetch count
        /// </summary>
        ISubscriptionBuilder WithPrefetchCount(ushort prefetchCount);

        /// <summary>
        /// Changes the prefetch size
        /// </summary>
        ISubscriptionBuilder WithPrefetchSize(uint prefetchSize);
    }
}