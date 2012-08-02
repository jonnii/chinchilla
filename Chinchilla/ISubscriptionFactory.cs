using System;
using RabbitMQ.Client;

namespace Chinchilla
{
    public interface ISubscriptionFactory
    {
        ISubscription Create<TMessage>(
            IModel model,
            Action<TMessage> handler,
            ISubscriptionConfiguration subscriptionConfiguration);
    }
}