namespace Chinchilla
{
    public class TaskWorker : Worker
    {
        private readonly IDeliveryProcessor connectedProcessor;

        public TaskWorker(IDeliveryProcessor connectedProcessor)
        {
            this.connectedProcessor = connectedProcessor;
        }

        public override string WorkerType
        {
            get { return "TaskWorker"; }
        }

        public void Deliver(IDelivery delivery)
        {
            connectedProcessor.Process(delivery);
        }
    }
}