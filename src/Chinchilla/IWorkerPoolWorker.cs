namespace Chinchilla
{
    public interface IWorkerPoolWorker : IWorker
    {
        void Start();

        void Stop();

        void Join();

        void Pause();

        void Resume();
    }
}