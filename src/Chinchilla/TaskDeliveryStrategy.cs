using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace Chinchilla
{
    public class TaskDeliveryStrategy : DeliveryStrategy
    {
        private readonly ConcurrentDictionary<int, TaskWorker> workers;

        public TaskDeliveryStrategy()
        {
            workers = new ConcurrentDictionary<int, TaskWorker>();
        }

        public int NumWorkers
        {
            get { return workers.Count; }
        }

        public override void Deliver(IDelivery delivery)
        {
            DeliverOnTask(delivery);
        }

        public override WorkerState[] GetWorkerStates()
        {
            return workers.Values.Select(v => v.GetState()).ToArray();
        }

        public Task DeliverOnTask(IDelivery delivery)
        {
            var worker = new TaskWorker(connectedProcessor);
            workers.TryAdd(worker.GetHashCode(), worker);

            var currentDelivery = delivery;

            return Task
                .Factory
                .StartNew(
                    () =>
                    {
                        worker.BeforeDeliver();
                        worker.Deliver(currentDelivery);
                    })
                .ContinueWith(
                    t =>
                    {
                        if (t.IsFaulted)
                        {
                            currentDelivery.Failed(t.Exception);
                        }
                        else
                        {
                            currentDelivery.Accept();
                        }
                        worker.AfterDeliver();

                        TaskWorker removed;
                        workers.TryRemove(worker.GetHashCode(), out removed);
                    });
        }
    }
}