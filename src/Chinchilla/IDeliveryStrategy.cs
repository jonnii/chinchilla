namespace Chinchilla
{
    public interface IDeliveryStrategy
    {
        void ConnectTo(IDeliveryProcessor processor);

        bool IsStartable { get; }

        void Start();

        void Stop();

        void Deliver(IDelivery delivery);

        WorkerState[] GetWorkerStates();
    }
}