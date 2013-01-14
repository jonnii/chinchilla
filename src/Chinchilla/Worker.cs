using System;

namespace Chinchilla
{
    public abstract class Worker
    {
        protected Worker()
        {
            Status = WorkerStatus.Stopped;
        }

        public abstract string WorkerType { get; }

        public WorkerStatus Status { get; set; }

        public DateTime? BusySince { get; set; }

        public WorkerState GetState()
        {
            return new WorkerState(WorkerType, Status, BusySince);
        }

        public void BeforeDeliver()
        {
            Status = WorkerStatus.Busy;
            BusySince = DateTime.UtcNow;
        }

        public void AfterDeliver()
        {
            Status = WorkerStatus.Idle;
            BusySince = null;
        }

        public IDisposable StartWorkingScope()
        {
            BeforeDeliver();
            return new ActionDisposable(AfterDeliver);
        }
    }
}