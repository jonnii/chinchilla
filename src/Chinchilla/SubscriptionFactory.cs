using System;
using System.Collections.Generic;
using System.Linq;
using Chinchilla.Configuration;
using Chinchilla.Logging;
using Chinchilla.Topologies.Model;

namespace Chinchilla
{
    public class SubscriptionFactory : TrackableFactory<Subscription>, ISubscriptionFactory
    {
        private readonly ILogger logger = Logger.Create<SubscriptionFactory>();

        private readonly IMessageSerializer messageSerializer;

        public SubscriptionFactory(
            IMessageSerializer messageSerializer)
        {
            this.messageSerializer = messageSerializer;
        }

        public ISubscription Create<TMessage>(
            IBus bus,
            IModelReference modelReference,
            ISubscriptionConfiguration configuration,
            Action<TMessage, IDeliveryContext> callback)
        {
            logger.DebugFormat("Creating new handler subscription with configuration: {0}", configuration);

            var deliveryProcessor = new ActionDeliveryProcessor<TMessage>(
                bus,
                messageSerializer,
                callback);

            var messageType = typeof(TMessage).Name;

            var subscriptionQueues = GetSubscriptionQueuesForEndpoints(
                messageType,
                modelReference,
                configuration);

            var deliveryStrategy = configuration.BuildDeliveryStrategy(deliveryProcessor);
            var faultStrategy = configuration.BuildFaultStrategy(bus);

            var subscription = new Subscription(
                modelReference,
                deliveryStrategy,
                faultStrategy,
                subscriptionQueues)
            {
                PrefetchSize = configuration.PrefetchSize,
                PrefetchCount = configuration.PrefetchCount
            };

            return Create(subscription);
        }

        private IEnumerable<IQueue> GetSubscriptionQueuesForEndpoints(
            string messageType,
            IModelReference modelReference,
            ISubscriptionConfiguration configuration)
        {
            var endpointNames = configuration.EndpointNames.Any()
                ? configuration.EndpointNames
                : new[] { messageType };

            foreach (var endpointName in endpointNames)
            {
                var endpoint = new Endpoint(endpointName, messageType);

                var topologyBuilder = new TopologyBuilder(modelReference);

                var topology = configuration.BuildTopology(endpoint);
                topology.Visit(topologyBuilder);

                yield return topology.SubscribeQueue;
            }
        }

        public ISubscription Create(
            Subscription subscription)
        {
            Track(subscription);
            return subscription;
        }

        public override void Dispose()
        {
            logger.Debug("Disposing of subscription factory");

            base.Dispose();
        }
    }
}