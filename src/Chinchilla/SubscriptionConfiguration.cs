using System;
using Chinchilla.Topologies;

namespace Chinchilla
{
    public class SubscriptionConfiguration : ISubscriptionConfiguration, ISubscriptionBuilder
    {
        public static SubscriptionConfiguration Default
        {
            get { return new SubscriptionConfiguration(); }
        }

        private Func<IDeliveryProcessor, IDeliveryStrategy> strategyBuilder = handler => new ImmediateDeliveryStrategy();

        public ISubscriptionBuilder DeliverUsing<TStrategy>(params Action<TStrategy>[] configurations)
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

            return this;
        }

        public IDeliveryStrategy BuildDeliveryStrategy(IDeliveryProcessor processor)
        {
            var consumer = strategyBuilder(processor);
            consumer.ConnectTo(processor);
            return consumer;
        }

        public ISubscriptionTopology BuildTopology<TMessage>()
        {
            return new DefaultSubscriptionTopology(typeof(TMessage).Name);
        }

        public override string ToString()
        {
            return string.Format("[SubscriptionConfiguration]");
        }
    }
}