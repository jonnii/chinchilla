namespace Chinchilla
{
    public interface IWorker
    {
        string Name { get; }

        WorkerState GetState();
    }
}