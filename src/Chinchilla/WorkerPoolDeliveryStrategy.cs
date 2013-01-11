using System.Collections.Concurrent;
using System.Linq;
using Chinchilla.Logging;

namespace Chinchilla
{
    public class WorkerPoolDeliveryStrategy : DeliveryStrategy
    {
        private readonly ILogger logger = Logger.Create<WorkerPoolDeliveryStrategy>();

        private readonly BlockingCollection<IDelivery> deliveries = new BlockingCollection<IDelivery>(
            new ConcurrentQueue<IDelivery>());

        private WorkerPoolThread[] threads = new WorkerPoolThread[0];

        public WorkerPoolDeliveryStrategy()
        {
            NumWorkers = 1;
        }

        public int NumWorkers { get; set; }

        public override bool IsStartable
        {
            get { return NumWorkers > 0; }
        }

        public override void Start()
        {
            logger.DebugFormat("Starting {0}", this);

            if (NumWorkers <= 0)
            {
                throw new ChinchillaException(
                    "Could not start the worker pool delivery queue " +
                    "because the number of configured worker threads is zero");
            }

            threads = Enumerable
                .Range(0, NumWorkers)
                .Select(_ => new WorkerPoolThread(deliveries, connectedProcessor))
                .ToArray();

            foreach (var thread in threads)
            {
                thread.Start();
            }
        }

        public override void Deliver(IDelivery delivery)
        {
            deliveries.Add(delivery);
        }

        public override WorkerState[] GetWorkerStates()
        {
            return threads.Select(t => t.GetState()).ToArray();
        }

        public override void Stop()
        {
            logger.DebugFormat("Stopping {0}", this);

            foreach (var thread in threads)
            {
                thread.IsStopping = true;
            }

            deliveries.CompleteAdding();

            foreach (var thread in threads)
            {
                thread.Join();
            }

            logger.DebugFormat("Stopped {0}", this);
        }

        public override string ToString()
        {
            return string.Format("[WorkerPoolDeliveryStrategy NumWorkers={0}]", NumWorkers);
        }
    }
}