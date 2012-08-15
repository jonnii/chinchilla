using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using Chinchilla.Logging;

namespace Chinchilla
{
    public class WorkerPoolDeliveryStrategy : DeliveryStrategy
    {
        private readonly ILogger logger = Logger.Create<WorkerPoolDeliveryStrategy>();

        private readonly BlockingCollection<IDelivery> deliveries = new BlockingCollection<IDelivery>(
            new ConcurrentQueue<IDelivery>());

        private Thread[] threads;

        private bool disposed;

        public WorkerPoolDeliveryStrategy()
        {
            NumWorkers = 1;
        }

        public int NumWorkers { get; set; }

        public override void Start()
        {
            logger.DebugFormat("Starting {0}", this);

            threads = Enumerable.Range(0, NumWorkers).Select(_ => new Thread(StartTakingMessages)).ToArray();
            foreach (var thread in threads)
            {
                thread.Start();
            }
        }

        public override void Deliver(IDelivery delivery)
        {
            deliveries.Add(delivery);
        }

        public void StartTakingMessages()
        {
            while (!disposed)
            {
                IDelivery delivery;

                try
                {
                    delivery = deliveries.Take();
                }
                catch (InvalidOperationException)
                {
                    break;
                }

                DeliverOne(delivery);
            }
        }

        public void DeliverOne(IDelivery delivery)
        {
            connectedProcessor.Process(delivery);
            delivery.Accept();
        }

        public override void Dispose()
        {
            disposed = true;
            deliveries.CompleteAdding();

            logger.DebugFormat("Disposing of {0}", this);
        }

        public override string ToString()
        {
            return string.Format("[WorkerPoolDeliveryStrategy NumWorkers={0}]", NumWorkers);
        }
    }
}