using System;

namespace Chinchilla
{
    public class IgnoreFaultStrategy : ISubscriptionFailureStrategy
    {
        public static ISubscriptionFailureStrategy Build(IBus bus)
        {
            return new IgnoreFaultStrategy();
        }

        public void OnFailure(IDelivery delivery, Exception exception)
        {
            delivery.Accept();
        }
    }
}