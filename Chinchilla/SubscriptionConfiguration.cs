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
            where TStrategy : IConsumerStrategy
        {

        }

        public IConsumerStrategy BuildConsumerStrategy(IDeliveryHandler deliveryHandler)
        {
            return new ImmediateConsumerStrategy(deliveryHandler);
        }

        public override string ToString()
        {
            return string.Format("[SubscriptionConfiguration]");
        }
    }
}