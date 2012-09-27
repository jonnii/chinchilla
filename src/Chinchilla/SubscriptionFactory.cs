using System;
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
            var endpoint = new Endpoint(configuration.EndpointNames.FirstOrDefault() ?? messageType, messageType);

            var topologyBuilder = new TopologyBuilder(modelReference);

            var topology = configuration.BuildTopology(endpoint);
            topology.Visit(topologyBuilder);

            var deliveryStrategy = configuration.BuildDeliveryStrategy(deliveryProcessor);
            var faultStrategy = configuration.BuildFaultStrategy(bus);

            var subscription = new Subscription(
                modelReference,
                deliveryStrategy,
                faultStrategy,
                new[] { topology.SubscribeQueue })
            {
                PrefetchSize = configuration.PrefetchSize,
                PrefetchCount = configuration.PrefetchCount
            };

            return Create(subscription);
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