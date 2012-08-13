using System;
using Chinchilla.Logging;

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

        public ISubscription Create<TMessage>(IModelReference modelReference, ISubscriptionConfiguration configuration, Action<TMessage> processor)
        {
            logger.DebugFormat("Creating new handler subscription with configuration: {0}", configuration);

            var deliveryHandler = new ActionDeliveryProcessor<TMessage>(
                messageSerializer,
                processor);

            var consumerStrategy = configuration.BuildDeliveryStrategy(deliveryHandler);

            return new Subscription<TMessage>(
                modelReference,
                consumerStrategy);
        }
    }
}