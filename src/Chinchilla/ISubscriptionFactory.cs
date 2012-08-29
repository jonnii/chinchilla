using System;

namespace Chinchilla
{
    public interface ISubscriptionFactory : IDisposable
    {
        ISubscription Create<TMessage>(
            IModelReference modelReference,
            ISubscriptionConfiguration configuration,
            Action<TMessage, IMessageContext> callback);
    }
}