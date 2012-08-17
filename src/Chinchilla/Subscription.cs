using System;
using System.Collections.Concurrent;
using System.Threading;
using Chinchilla.Logging;
using Chinchilla.Topologies.Model;
using RabbitMQ.Client.Events;

namespace Chinchilla
{
    public class Subscription : Trackable, ISubscription, IDeliveryListener
    {
        private readonly ILogger logger = Logger.Create<Subscription>();

        private readonly IModelReference modelReference;

        private readonly IDeliveryStrategy deliveryStrategy;

        private BlockingCollection<BasicDeliverEventArgs> consumerQueue;

        private Thread subscriptionThread;

        private bool disposed;

        public Subscription(
            IModelReference modelReference,
            IDeliveryStrategy deliveryStrategy,
            IQueue queue)
        {
            this.modelReference = modelReference;
            this.deliveryStrategy = deliveryStrategy;

            Queue = queue;
        }

        public ulong NumAcceptedMessages { get; private set; }

        public IQueue Queue { get; private set; }

        public void Start()
        {
            modelReference.Execute(m => m.BasicQos(0, 0, false));

            logger.Debug("Creating Consumer");
            consumerQueue = modelReference.GetConsumerQueue(Queue);

            logger.Debug("Starting listener thread");
            subscriptionThread = new Thread(() =>
            {
                while (!disposed)
                {
                    BasicDeliverEventArgs item;
                    try
                    {
                        item = consumerQueue.Take();
                    }
                    catch (InvalidOperationException)
                    {
                        break;
                    }

                    var delivery = new Delivery(this, item.DeliveryTag, item.Body);
                    deliveryStrategy.Deliver(delivery);
                }

                logger.DebugFormat("Subscription thread terminated for: {0}", this);
            });

            subscriptionThread.Start();
            deliveryStrategy.Start();
        }

        public void OnAccept(IDelivery delivery)
        {
            modelReference.Execute(
                m => m.BasicAck(delivery.Tag, false));

            ++NumAcceptedMessages;
        }

        public override void Dispose()
        {
            logger.DebugFormat("Disposing {0}", this);

            disposed = true;
            deliveryStrategy.Dispose();
            modelReference.Dispose();

            base.Dispose();
        }

        public override string ToString()
        {
            return "[Subscription]";
        }
    }
}
