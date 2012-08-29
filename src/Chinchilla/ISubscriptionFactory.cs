using System;

namespace Chinchilla
{
    public interface ISubscriptionFactory : IDisposable
    {
        ISubscription Create<TMessage>(
            IBus bus,
            IModelReference modelReference,
            ISubscriptionConfiguration configuration,
            Action<TMessage, IDeliveryContext> callback);
    }
}