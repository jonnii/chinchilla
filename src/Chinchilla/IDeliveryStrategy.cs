using System;

namespace Chinchilla
{
    public interface IDeliveryStrategy : IDisposable
    {
        void ConnectTo(IDeliveryHandler handler);

        void Start();

        void Deliver(IDelivery delivery);
    }
}