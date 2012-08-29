using System;

namespace Chinchilla
{
    public interface IDeliveryListener
    {
        void OnAccept(IDelivery delivery);

        void OnFailed(IDelivery delivery, Exception exception);
    }
}