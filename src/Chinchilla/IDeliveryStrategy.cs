using System;

namespace Chinchilla
{
    public interface IDeliveryStrategy : IDisposable
    {
        void ConnectTo(IDeliveryProcessor processor);

        bool IsStartable { get; }

        void Start();

        void Deliver(IDelivery delivery);
    }
}