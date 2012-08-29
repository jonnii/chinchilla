using System;

namespace Chinchilla
{
    public class ErrorQueueDeliveryFailureStrategy : IDeliveryFailureStrategy
    {
        public void Handle(IDelivery delivery, Exception exception)
        {

        }
    }
}