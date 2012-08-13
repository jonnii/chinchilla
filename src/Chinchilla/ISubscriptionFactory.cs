using System;

namespace Chinchilla
{
    public interface ISubscriptionFactory
    {
        ISubscription Create<TMessage>(
            IModelReference modelReference,
            ISubscriptionConfiguration configuration,
            Action<TMessage> processor);
    }
}