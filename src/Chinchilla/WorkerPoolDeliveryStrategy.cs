using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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

        private Thread[] threads = new Thread[0];

        private bool isStopping;

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
                .Select(_ => new Thread(StartTakingMessages))
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
            return new WorkerState[0];

            //return threads.Select(t => t.GetState()).ToArray();
        }

        public void StartTakingMessages()
        {
            while (!isStopping)
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
            try
            {
                connectedProcessor.Process(delivery);
            }
            catch (Exception e)
            {
                delivery.Failed(e);
                return;
            }

            delivery.Accept();
        }

        public override void Stop()
        {
            logger.DebugFormat("Stopping {0}", this);

            isStopping = true;

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