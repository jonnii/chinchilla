using System;
using Chinchilla.Logging;
using RabbitMQ.Client;

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

        public ISubscription Create<TMessage>(IModel model, Action<TMessage> handler)
        {
            logger.Debug("Creating new handler subscription");

            var actionHandler = new ActionMessageHandler<TMessage>(handler);

            return new Subscription<TMessage>(
                model,
                messageSerializer,
                actionHandler);
        }
    }
}