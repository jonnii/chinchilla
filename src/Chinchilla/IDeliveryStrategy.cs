using System;

namespace Chinchilla
{
    public interface IDeliveryStrategy : IDisposable
    {
        void ConnectTo(IDeliveryProcessor processor);

        void Start();

        void Deliver(IDelivery delivery);
    }
}