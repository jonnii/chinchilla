using System.Linq;
using Chinchilla.Logging;

namespace Chinchilla
{
    public class WorkerPoolWorkersController : IWorkersController
    {
        private readonly ILogger logger = Logger.Create<WorkerPoolWorkersController>();

        private readonly IWorkerPoolWorker[] workers;

        public WorkerPoolWorkersController(IWorkerPoolWorker[] workers)
        {
            this.workers = workers;
        }

        public void Pause(string workerName)
        {
            logger.InfoFormat("Trying to pause worker: {0}", workerName);

            var worker = FindWorkerByName(workerName);
            worker.Pause();
        }

        public void Resume(string workerName)
        {
            logger.InfoFormat("Trying to resume worker: {0}", workerName);

            var worker = FindWorkerByName(workerName);
            worker.Resume();
        }

        private IWorkerPoolWorker FindWorkerByName(string workerName)
        {
            var worker = workers.FirstOrDefault(w => w.Name == workerName);

            if (worker == null)
            {
                var message = string.Format(
                    "Could not pause a worker with name {0} because it could not be found. The available workers are: {1}",
                    workerName,
                    string.Join(", ", workers.Select(w => w.Name)));

                throw new ChinchillaException(message);
            }
            return worker;
        }
    }
}