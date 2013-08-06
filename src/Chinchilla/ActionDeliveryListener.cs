using System;

namespace Chinchilla
{
    /// <summary>
    /// An action delivery listener is a convenience class used for registering
    /// actions to be called when a delivery listener is triggered.
    /// </summary>
    public class ActionDeliveryListener : IDeliveryListener
    {
        private readonly Action action;

        public ActionDeliveryListener(Action action)
        {
            this.action = action;
        }

        public void OnAccept(IDelivery delivery)
        {
            action();
        }

        public void OnFailed(IDelivery delivery, Exception exception)
        {
            action();
        }
    }
}