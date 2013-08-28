using System;

namespace Chinchilla
{
    /// <summary>
    /// A delivery listener is used to listen to a completion event on a delivery
    /// </summary>
    public interface IDeliveryListener
    {
        /// <summary>
        /// OnAccept will be called when a delivery is accepted
        /// </summary>
        void OnAccept(IDelivery delivery);

        /// <summary>
        /// OnReject will be called when a delivery is rejected
        /// </summary>
        void OnReject(IDelivery delivery, bool requeue);

        /// <summary>
        /// OnFailed will be called when a delivery is failed
        /// </summary>
        void OnFailed(IDelivery delivery, Exception exception);
    }
}