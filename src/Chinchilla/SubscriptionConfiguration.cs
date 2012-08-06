using System;

namespace Chinchilla
{
    public class SubscriptionConfiguration : ISubscriptionConfiguration
    {
        public static ISubscriptionConfiguration Default
        {
            get { return new SubscriptionConfiguration(); }
        }

        private Func<IDeliveryHandler, IDeliveryStrategy> strategyBuilder = handler => new ImmediateDeliveryStrategy();

        public void DeliverUsing<TStrategy>(params Action<TStrategy>[] configurations)
            where TStrategy : IDeliveryStrategy, new()
        {
            strategyBuilder = handler =>
            {
                var strategy = new TStrategy();
                foreach (var configuration in configurations)
                {
                    configuration(strategy);
                }
                return strategy;
            };
        }

        public IDeliveryStrategy BuildDeliveryStrategy(IDeliveryHandler handler)
        {
            var consumer = strategyBuilder(handler);
            consumer.ConnectTo(handler);
            return consumer;
        }

        public override string ToString()
        {
            return string.Format("[SubscriptionConfiguration]");
        }
    }
}