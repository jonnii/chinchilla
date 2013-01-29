namespace Chinchilla
{
    public interface IWorkersController
    {
        void Pause(string workerName);

        void Resume(string workerName);
    }
}