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

        public SubscriptionFactory(IMessageSerializer messageSerializer)
        {
            this.messageSerializer = messageSerializer;
        }

        public ISubscription Create<TMessage>(
            IModelReference modelReference,
            ISubscriptionConfiguration configuration,
            Action<TMessage> processor)
        {
            logger.DebugFormat("Creating new handler subscription with configuration: {0}", configuration);

            var deliveryProcessor = new ActionDeliveryProcessor<TMessage>(messageSerializer, processor);

            var topologyBuilder = new TopologyBuilder(modelReference);

            var messageType = typeof(TMessage).Name;
            var topology = configuration.BuildTopology(messageType);
            topology.Visit(topologyBuilder);

            var consumerStrategy = configuration.BuildDeliveryStrategy(deliveryProcessor);

            return Create(modelReference, consumerStrategy, topology);
        }

        public ISubscription Create(IModelReference modelReference, IDeliveryStrategy deliveryStrategy, ISubscriberTopology topology)
        {
            var subscription = new Subscription(modelReference, deliveryStrategy, topology.SubscribeQueue);
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