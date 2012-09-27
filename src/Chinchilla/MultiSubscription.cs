using System;
using System.Collections.Generic;
using Chinchilla.Topologies.Model;

namespace Chinchilla
{
    public class MultiSubscription : ISubscription
    {
        private readonly IList<ISubscription> subscriptions;

        public MultiSubscription(IList<ISubscription> subscriptions)
        {
            this.subscriptions = subscriptions;
        }

        public void Dispose()
        {
            foreach (var subscription in subscriptions)
            {
                subscription.Dispose();
            }
        }

        public ulong NumAcceptedMessages
        {
            get { throw new NotImplementedException(); }
        }

        public ulong NumFailedMessages
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<IQueue> Queues
        {
            get { throw new NotImplementedException(); }
        }

        public void Start()
        {
            foreach (var subscription in subscriptions)
            {
                subscription.Start();
            }
        }
    }
}