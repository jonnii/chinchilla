using System;
using System.Collections.Concurrent;
using System.Threading;
using Chinchilla.Threading;

namespace Chinchilla
{
    public class WorkerPoolWorker : Worker, IWorkerPoolWorker
    {
        private readonly int ordinal;

        private readonly BlockingCollection<IDelivery> deliveries;

        private readonly IThread thread;

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        private readonly ManualResetEventSlim pauseEvent = new ManualResetEventSlim(true);

        public WorkerPoolWorker(
            int ordinal,
            IThreadFactory threadFactory,
            BlockingCollection<IDelivery> deliveries,
            IDeliveryProcessor connectedProcessor)
            : base(connectedProcessor)
        {
            this.ordinal = ordinal;
            this.deliveries = deliveries;

            thread = threadFactory.Create(StartTakingMessages);
        }

        public int WorkerIndex { get; set; }

        public override string Name
        {
            get { return string.Concat("pool-worker-", ordinal); }
        }

        public override string Type
        {
            get { return "WorkerPool"; }
        }

        public bool IsStopping { get; set; }

        public void Start()
        {
            if (Status != WorkerStatus.Stopped)
            {
                throw new InvalidOperationException(
                    "Cannot start this worker because it has already been started");
            }

            Status = WorkerStatus.Starting;
            thread.Start();
        }

        public void Join()
        {
            thread.Join();
        }

        public void Pause()
        {
            pauseEvent.Reset();
            cancellationTokenSource.Cancel();
            Status = WorkerStatus.Paused;
        }

        public void Resume()
        {
            if (Status != WorkerStatus.Paused)
            {
                throw new InvalidOperationException(
                    "Cannot resume this worker because it has not yet been paused");
            }

            Status = WorkerStatus.Idle;
            pauseEvent.Set();
        }

        public void Stop()
        {
            pauseEvent.Set();
            IsStopping = true;
            Status = WorkerStatus.Stopping;
        }

        public void StopAndJoin()
        {
            Stop();
            thread.Join();
        }

        public void BeforeStartMessagePump()
        {
            Status = WorkerStatus.Idle;
        }

        public void StartTakingMessages()
        {
            BeforeStartMessagePump();
            PumpMessages();
        }

        private void PumpMessages()
        {
            while (!IsStopping)
            {
                pauseEvent.Wait();

                IDelivery delivery;

                try
                {
                    delivery = deliveries.Take(cancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    continue;
                }
                catch (InvalidOperationException)
                {
                    break;
                }

                Deliver(delivery);
            }

            Status = WorkerStatus.Stopped;
        }
    }
}