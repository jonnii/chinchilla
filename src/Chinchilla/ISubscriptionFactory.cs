using System;
using Chinchilla.Configuration;

namespace Chinchilla
{
    public interface ISubscriptionFactory : IDisposable
    {
        ISubscription Create<TMessage>(
            IBus bus,
            ISubscriptionConfiguration configuration,
            Action<TMessage, IDeliveryContext> callback);
    }
}