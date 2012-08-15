using System;
using Chinchilla.Logging;
using Chinchilla.Topologies.Rabbit;

namespace Chinchilla
{
    public class SubscriptionFactory : ISubscriptionFactory
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

            var deliveryHandler = new ActionDeliveryProcessor<TMessage>(
                messageSerializer,
                processor);

            logger.Debug("Creating topology");

            var topologyBuilder = new TopologyBuilder(modelReference);

            var messageType = typeof(TMessage).Name;
            var topology = configuration.BuildTopology(messageType);
            topology.Visit(topologyBuilder);

            var consumerStrategy = configuration.BuildDeliveryStrategy(deliveryHandler);

            return new Subscription<TMessage>(
                modelReference,
                consumerStrategy,
                topology.Queue);
        }

        public void Dispose()
        {
        }
    }
}