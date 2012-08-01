using System;
using RabbitMQ.Client;

namespace Chinchilla
{
    public class SubscriptionFactory : ISubscriptionFactory
    {
        private readonly IMessageSerializer messageSerializer;

        public SubscriptionFactory(IMessageSerializer messageSerializer)
        {
            this.messageSerializer = messageSerializer;
        }

        public ISubscription Create<TMessage>(IModel model, Action<TMessage> handler)
        {
            var actionHandler = new ActionMessageHandler<TMessage>(handler);

            return new Subscription<TMessage>(
                model,
                messageSerializer,
                actionHandler);
        }
    }
}