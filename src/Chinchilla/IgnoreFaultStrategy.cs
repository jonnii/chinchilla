using System;

namespace Chinchilla
{
    public class IgnoreFaultStrategy : IFaultStrategy
    {
        public static IFaultStrategy Build(IBus bus)
        {
            return new IgnoreFaultStrategy();
        }

        public void Handle(IDelivery delivery, Exception exception)
        {
            delivery.Accept();
        }
    }
}