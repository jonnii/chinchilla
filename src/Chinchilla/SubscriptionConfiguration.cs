using System;
using Chinchilla.Topologies;

namespace Chinchilla
{
    public class SubscriptionConfiguration : ISubscriptionConfiguration, ISubscriptionBuilder
    {
        private Func<IDeliveryProcessor, IDeliveryStrategy> strategyBuilder = handler => new ImmediateDeliveryStrategy();

        private Func<Endpoint, ISubscriberTopology> topologyBuilder = endpoint => new DefaultTopology(endpoint);

        public string QueueName { get; private set; }

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

        public ISubscriptionBuilder SetTopology(Func<Endpoint, ISubscriberTopology> customTopologyBuilder)
        {
            topologyBuilder = customTopologyBuilder;
            return this;
        }

        public ISubscriptionBuilder SubscribeOn(string subscriptionQueueName)
        {
            QueueName = subscriptionQueueName;
            return this;
        }

        public ISubscriberTopology BuildTopology(Endpoint endpoint)
        {
            return topologyBuilder(endpoint);
        }

        public override string ToString()
        {
            return string.Format("[SubscriptionConfiguration]");
        }
    }
}