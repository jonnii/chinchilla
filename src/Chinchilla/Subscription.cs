using System.Linq;
using System.Threading;
using Chinchilla.Logging;
using RabbitMQ.Client.Events;

namespace Chinchilla
{
    public class Subscription : Trackable, ISubscription
    {
        private readonly ILogger logger = Logger.Create<Subscription>();

        private readonly IDeliveryStrategy deliveryStrategy;

        private Thread listenerThread;

        private bool disposed;

        public Subscription(
            IDeliveryStrategy deliveryStrategy,
            IDeliveryQueue[] queues)
        {
            this.deliveryStrategy = deliveryStrategy;

            Queues = queues;
        }

        public IDeliveryQueue[] Queues { get; private set; }

        public bool IsStarted { get; private set; }

        public bool IsStartable
        {
            get { return deliveryStrategy.IsStartable; }
        }

        public long NumAcceptedMessages
        {
            get { return Queues.Sum(q => q.NumAcceptedMessages); }
        }

        public long NumFailedMessages
        {
            get { return Queues.Sum(q => q.NumFailedMessages); }
        }

        public void Start()
        {
            logger.InfoFormat("Starting subscription: {0}", this);

            logger.Debug(" -> Building listener thread");
            listenerThread = BuildListenerThread();

            logger.Debug(" -> starting delivery strategy");
            deliveryStrategy.Start();

            logger.Debug(" -> starting all queues");
            foreach (var queue in Queues)
            {
                queue.Start();
            }

            logger.Debug(" -> Starting listener thread");
            listenerThread.Start();

            IsStarted = true;
        }

        private Thread BuildListenerThread()
        {
            return new Thread(() =>
            {
                while (!disposed)
                {
                    BasicDeliverEventArgs item = null;

                    // grab the first item where any of the consumer queues has a value

                    var queue = Queues
                        .FirstOrDefault(q => q.TryTake(out item));

                    // if we're dead and we didn't find an item, 
                    // it means it's time to pack up and go home

                    if (disposed && queue == null)
                    {
                        break;
                    }

                    // if we're not dead and we didn't find an item
                    // lets go around again

                    if (!disposed && queue == null)
                    {
                        continue;
                    }

                    if (item == null)
                    {
                        continue;
                    }

                    var delivery = new Delivery(
                        queue,
                        item.DeliveryTag,
                        item.Body,
                        item.RoutingKey,
                        item.Exchange);

                    deliveryStrategy.Deliver(delivery);
                }

                logger.DebugFormat("Subscription thread terminated for: {0}", this);
            });
        }

        public override void Dispose()
        {
            logger.DebugFormat("Disposing {0}", this);

            if (disposed)
            {
                return;
            }

            disposed = true;

            // complete adding on the consumer queue and wait for the subscription
            // thread to complete, this will give any processing jobs a change to complete
            if (Queues != null)
            {
                foreach (var queue in Queues)
                {
                    queue.Dispose();
                }
            }

            if (listenerThread != null && listenerThread.IsAlive)
            {
                listenerThread.Join();
            }

            // dispose of delivery strategy and the underlying model
            // on this subscription
            deliveryStrategy.Dispose();

            base.Dispose();
        }

        public override string ToString()
        {
            var queueNames = string.Join(",", Queues.Select(q => q.ToString()));
            return string.Format("[Subscription Queues={0}]", queueNames);
        }
    }
}
