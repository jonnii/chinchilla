using System;
using System.Collections.Generic;
using System.Linq;

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

        public long NumAcceptedMessages
        {
            get { throw new NotImplementedException(); }
        }

        public long NumFailedMessages
        {
            get { throw new NotImplementedException(); }
        }

        public IDeliveryQueue[] Queues
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsStartable
        {
            get
            {
                return subscriptions.All(s => s.IsStartable);
            }
        }

        public bool IsStarted
        {
            get
            {
                return subscriptions.All(s => s.IsStarted);
            }
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