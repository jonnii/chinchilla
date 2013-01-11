using System;
using System.Collections.Generic;
using Chinchilla.Configuration;

namespace Chinchilla
{
    public interface ISubscriptionFactory : IDisposable
    {
        IEnumerable<ISubscription> List();

        ISubscription Create<TMessage>(
            IBus bus,
            ISubscriptionConfiguration configuration,
            Action<TMessage, IDeliveryContext> callback);
    }
}