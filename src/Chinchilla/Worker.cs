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

        public WorkerState GetState()
        {
            return new WorkerState(WorkerType, Status);
        }

        public void BeforeDeliver()
        {
            Status = WorkerStatus.Busy;
        }

        public void AfterDeliver()
        {
            Status = WorkerStatus.Idle;
        }

        public IDisposable StartWorkingScope()
        {
            BeforeDeliver();
            return new ActionDisposable(AfterDeliver);
        }
    }
}