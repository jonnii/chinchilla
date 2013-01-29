namespace Chinchilla
{
    public interface IDeliveryStrategy
    {
        bool IsStartable { get; }

        void ConnectTo(IDeliveryProcessor processor);

        void Start();

        void Stop();

        void Deliver(IDelivery delivery);

        WorkerState[] GetWorkerStates();

        IWorkersController GetWorkersController();
    }
}