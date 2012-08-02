using System;

namespace Chinchilla
{
    public class SubscriptionConfiguration : ISubscriptionConfiguration
    {
        public static ISubscriptionConfiguration Default
        {
            get { return new SubscriptionConfiguration(); }
        }

        public void ConsumerStrategy<TStrategy>(params Action<TStrategy>[] configuration)
            where TStrategy : IDeliveryStrategy
        {

        }

        public IDeliveryStrategy BuildConsumerStrategy(IDeliveryHandler deliveryHandler)
        {
            return new ImmediateDeliveryStrategy(deliveryHandler);
        }

        public override string ToString()
        {
            return string.Format("[SubscriptionConfiguration]");
        }
    }
}