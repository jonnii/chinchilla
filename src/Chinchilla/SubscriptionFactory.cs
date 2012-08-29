using System;
using Chinchilla.Logging;
using Chinchilla.Topologies;
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

            var messageType = typeof(TMessage).Name;
            var endpoint = new Endpoint(configuration.QueueName ?? messageType, messageType);

            var topologyBuilder = new TopologyBuilder(modelReference);

            var topology = configuration.BuildTopology(endpoint);
            topology.Visit(topologyBuilder);

            var deliveryProcessor = new ActionDeliveryProcessor<TMessage>(
                bus,
                messageSerializer,
                callback);

            var deliveryStrategy = configuration.BuildDeliveryStrategy(deliveryProcessor);

            return Create(modelReference, deliveryStrategy, topology);
        }

        public ISubscription Create(
            IModelReference modelReference,
            IDeliveryStrategy deliveryStrategy,
            IMessageTopology messageTopology)
        {
            var subscription = new Subscription(modelReference, deliveryStrategy, messageTopology.SubscribeQueue);
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