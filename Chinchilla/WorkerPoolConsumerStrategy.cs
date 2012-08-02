namespace Chinchilla
{
    public class WorkerPoolConsumerStrategy : IConsumerStrategy
    {
        public int NumWorkers { get; set; }

        public void Deliver(IDelivery delivery)
        {
        }
    }
}