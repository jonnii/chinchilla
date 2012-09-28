using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Chinchilla.Logging;
using Chinchilla.Topologies.Model;
using RabbitMQ.Client.Events;

namespace Chinchilla
{
    public class Subscription : Trackable, ISubscription, IDeliveryListener
    {
        private const int ConsumerTakeTimeoutInMs = 50;

        private readonly ILogger logger = Logger.Create<Subscription>();

        private readonly IModelReference modelReference;

        private readonly IDeliveryStrategy deliveryStrategy;

        private readonly IFaultStrategy faultStrategy;

        private IEnumerable<BlockingCollection<BasicDeliverEventArgs>> consumerQueues;

        private Thread subscriptionThread;

        private bool disposed;

        public Subscription(
            IModelReference modelReference,
            IDeliveryStrategy deliveryStrategy,
            IFaultStrategy faultStrategy,
            IEnumerable<IQueue> queues)
        {
            this.modelReference = modelReference;
            this.deliveryStrategy = deliveryStrategy;
            this.faultStrategy = faultStrategy;

            Queues = queues;
        }

        public IEnumerable<IQueue> Queues { get; private set; }

        public uint PrefetchSize { get; set; }

        public ushort PrefetchCount { get; set; }

        public ulong NumAcceptedMessages { get; private set; }

        public ulong NumFailedMessages { get; private set; }

        public void Start()
        {
            logger.InfoFormat("Starting subscription: {0}", this);

            modelReference.Execute(m => m.BasicQos(PrefetchSize, PrefetchCount, false));

            // build a consumer for each queue

            consumerQueues = Queues
                .Select(q => modelReference.GetConsumerQueue(q))
                .ToArray();

            logger.Debug(" -> Starting listener thread");

            subscriptionThread = new Thread(() =>
            {
                while (!disposed)
                {
                    BasicDeliverEventArgs item = null;

                    // grab the first item where any of the consumer queues has a value

                    var itemTaken = consumerQueues
                        .Any(q => q.TryTake(out item, ConsumerTakeTimeoutInMs));

                    // if we're dead and we didn't find an item, 
                    // it means it's time to pack up and go home

                    if (disposed && !itemTaken)
                    {
                        break;
                    }

                    // if we're not dead and we didn't find an item
                    // lets go around again

                    if (!disposed && !itemTaken)
                    {
                        continue;
                    }

                    if (item == null)
                    {
                        continue;
                    }

                    var delivery = new Delivery(
                        this,
                        item.DeliveryTag,
                        item.Body,
                        item.RoutingKey,
                        item.Exchange);

                    deliveryStrategy.Deliver(delivery);
                }

                logger.DebugFormat("Subscription thread terminated for: {0}", this);
            });

            deliveryStrategy.Start();
            subscriptionThread.Start();
        }

        public void OnAccept(IDelivery delivery)
        {
            modelReference.Execute(
                m => m.BasicAck(delivery.Tag, false));

            ++NumAcceptedMessages;
        }

        public void OnFailed(IDelivery delivery, Exception exception)
        {
            faultStrategy.ProcessFailedDelivery(delivery, exception);

            ++NumFailedMessages;
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
            if (consumerQueues != null)
            {
                foreach (var consumerQueue in consumerQueues)
                {
                    consumerQueue.CompleteAdding();
                }
            }

            if (subscriptionThread != null)
            {
                subscriptionThread.Join();
            }

            // dispose of delivery strategy and the underlying model
            // on this subscription
            deliveryStrategy.Dispose();
            modelReference.Dispose();

            base.Dispose();
        }

        public override string ToString()
        {
            var queueNames = string.Join(",", Queues.Select(q => q.Name));
            return string.Format("[Subscription Queues={0}, PrefetchCount={1}, PrefetchSize={2}]", queueNames, PrefetchCount, PrefetchSize);
        }
    }
}
