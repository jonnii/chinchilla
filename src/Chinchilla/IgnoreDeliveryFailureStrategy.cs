using System;

namespace Chinchilla
{
    public class IgnoreDeliveryFailureStrategy : IDeliveryFailureStrategy
    {
        public void Handle(IDelivery delivery, Exception exception)
        {

        }
    }
}