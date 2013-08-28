using System;

namespace Chinchilla
{
    public interface IDeliveryListener
    {
        void OnAccept(IDelivery delivery);

        void OnReject(IDelivery delivery, bool requeue);

        void OnFailed(IDelivery delivery, Exception exception);
    }
}