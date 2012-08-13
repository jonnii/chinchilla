using System;
using System.Collections.Concurrent;
using System.Threading;
using Chinchilla.Logging;
using Chinchilla.Topologies.Rabbit;
using RabbitMQ.Client.Events;
using ExchangeType = Chinchilla.Topologies.Rabbit.ExchangeType;

namespace Chinchilla
{
    public class Subscription<T> : ISubscription, IDeliveryListener
    {
        private readonly ILogger logger = Logger.Create<Subscription<T>>();

        private readonly IModelReference modelReference;

        private readonly IDeliveryStrategy deliveryStrategy;

        private readonly Topology topology;

        private readonly TopologyBuilder topologyBuilder;

        private BlockingCollection<BasicDeliverEventArgs> consumerQueue;

        private Thread subscriptionThread;

        private bool disposed;

        public Subscription(
            IModelReference modelReference,
            IDeliveryStrategy deliveryStrategy)
        {
            this.modelReference = modelReference;
            this.deliveryStrategy = deliveryStrategy;

            topology = new Topology();
            topologyBuilder = new TopologyBuilder(modelReference);
        }

        public void Start()
        {
            var queue = topology.DefineQueue(typeof(T).Name);
            var exchange = topology.DefineExchange(typeof(T).Name, ExchangeType.Fanout);
            queue.BindTo(exchange);

            logger.Debug("Creating topology");

            topology.Visit(topologyBuilder);

            modelReference.Execute(m => m.BasicQos(0, 0, false));

            logger.Debug("Creating Consumer");
            consumerQueue = modelReference.GetConsumerQueue(queue);

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
        }

        public void Dispose()
        {
            logger.DebugFormat("Disposing {0}", this);

            disposed = true;
            deliveryStrategy.Dispose();
            modelReference.Dispose();
        }

        public override string ToString()
        {
            return "[Subscription]";
        }
    }
}
