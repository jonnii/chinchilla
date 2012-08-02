namespace Chinchilla
{
    public class WorkerPoolDeliveryStrategy : IDeliveryStrategy
    {
        public int NumWorkers { get; set; }

        public void Deliver(IDelivery delivery)
        {
        }
    }
}