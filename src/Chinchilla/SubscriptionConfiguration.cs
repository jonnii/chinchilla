using System;
using Chinchilla.Topologies;

namespace Chinchilla
{
    public class SubscriptionConfiguration : ISubscriptionConfiguration, ISubscriptionBuilder
    {
        private Func<IDeliveryProcessor, IDeliveryStrategy> strategyBuilder = handler => new ImmediateDeliveryStrategy();

        private Func<string, ISubscriptionTopology> topologyBuilder = messageType => new DefaultSubscriptionTopology(messageType);

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

        public void SetTopology(Func<string, ISubscriptionTopology> customTopologyBuilder)
        {
            topologyBuilder = customTopologyBuilder;
        }

        public ISubscriptionTopology BuildTopology(string messageType)
        {
            return topologyBuilder(messageType);
        }

        public override string ToString()
        {
            return string.Format("[SubscriptionConfiguration]");
        }
    }
}