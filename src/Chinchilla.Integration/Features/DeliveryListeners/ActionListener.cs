using System;

namespace Chinchilla.Integration.Features.DeliveryListeners
{
    public class ActionListener : IDeliveryListener
    {
        private readonly Action onAccept;

        public ActionListener(Action onAccept)
        {
            this.onAccept = onAccept;
        }

        public void OnAccept(IDelivery delivery)
        {
            onAccept();
        }

        public void OnFailed(IDelivery delivery, Exception exception)
        {
        }
    }
}
