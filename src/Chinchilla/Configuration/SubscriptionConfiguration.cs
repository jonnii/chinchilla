using System;
using Chinchilla.Topologies;

namespace Chinchilla.Configuration
{
    public class SubscriptionConfiguration : EndpointConfiguration, ISubscriptionConfiguration, ISubscriptionBuilder
    {
        private Func<IDeliveryProcessor, IDeliveryStrategy> strategyBuilder = handler => new ImmediateDeliveryStrategy();

        private Func<IDeliveryFailureStrategy> failureStrategyBuilder = () => new ErrorQueueDeliveryFailureStrategy();

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

        public ISubscriptionBuilder DeliverFailuresUsing<TStrategy>(params Action<TStrategy>[] configurations)
            where TStrategy : IDeliveryFailureStrategy, new()
        {
            failureStrategyBuilder = () =>
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

        public IDeliveryFailureStrategy BuildDeliveryFailureStrategy()
        {
            return failureStrategyBuilder();
        }

        public ISubscriptionBuilder SetTopology(IMessageTopologyBuilder messageTopologyBuilder)
        {
            MessageTopologyBuilder = messageTopologyBuilder;
            return this;
        }

        public ISubscriptionBuilder SetTopology<TBuilder>()
            where TBuilder : IMessageTopologyBuilder, new()
        {
            SetTopology(new TBuilder());
            return this;
        }

        public ISubscriptionBuilder SubscribeOn(string subscriptionQueueName)
        {
            QueueName = subscriptionQueueName;
            return this;
        }

        public override string ToString()
        {
            return string.Format("[SubscriptionConfiguration]");
        }
    }
}