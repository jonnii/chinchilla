using System;

namespace Chinchilla
{
    public class IgnoreDeliveryFailureStrategy : IDeliveryFailureStrategy
    {
        public static IDeliveryFailureStrategy Build(IBus bus)
        {
            return new IgnoreDeliveryFailureStrategy();
        }

        public void Handle(IDelivery delivery, Exception exception)
        {
            delivery.Accept();
        }
    }
}