namespace Chinchilla
{
    public class TaskWorker : Worker
    {
        public TaskWorker(IDeliveryProcessor connectedProcessor)
            : base(connectedProcessor)
        {
        }

        public override string WorkerType
        {
            get { return "TaskWorker"; }
        }
    }
}