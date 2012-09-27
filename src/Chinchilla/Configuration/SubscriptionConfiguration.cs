using System;
using System.Collections.Generic;
using System.Linq;
using Chinchilla.Topologies;

namespace Chinchilla.Configuration
{
    public class SubscriptionConfiguration : EndpointConfiguration, ISubscriptionConfiguration, ISubscriptionBuilder
    {
        private Func<IDeliveryProcessor, IDeliveryStrategy> strategyBuilder = handler => new ImmediateDeliveryStrategy();

        private Func<IBus, IFaultStrategy> failureStrategyBuilder = bus => new ErrorQueueFaultStrategy(bus);

        public SubscriptionConfiguration()
        {
            MessageTopologyBuilder = new DefaultSubscribeTopologyBuilder();
            QueueNames = Enumerable.Empty<string>();
            PrefetchSize = 0;
            PrefetchCount = 50;
        }

        public IEnumerable<string> QueueNames { get; private set; }

        public uint PrefetchSize { get; private set; }

        public ushort PrefetchCount { get; private set; }

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

        public ISubscriptionBuilder DeliverFaultsUsing<TStrategy>(params Action<TStrategy>[] configurations)
            where TStrategy : IFaultStrategy, new()
        {
            failureStrategyBuilder = bus =>
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

        public IFaultStrategy BuildFaultStrategy(IBus bus)
        {
            return failureStrategyBuilder(bus);
        }

        public ISubscriptionBuilder SetTopology(IMessageTopologyBuilder builder)
        {
            MessageTopologyBuilder = builder;
            return this;
        }

        public ISubscriptionBuilder SetTopology<TBuilder>()
            where TBuilder : IMessageTopologyBuilder, new()
        {
            SetTopology(new TBuilder());
            return this;
        }

        public ISubscriptionBuilder SubscribeOn(string queueName, params string[] otherQueueNames)
        {
            QueueNames = new[] { queueName }.Concat(otherQueueNames);

            return this;
        }

        public ISubscriptionBuilder WithPrefetchCount(ushort prefetchCount)
        {
            PrefetchCount = prefetchCount;
            return this;
        }

        public ISubscriptionBuilder WithPrefetchSize(uint prefetchSize)
        {
            PrefetchSize = prefetchSize;
            return this;
        }

        public override string ToString()
        {
            var queueNames = QueueNames.Any()
                ? string.Join(",", QueueNames)
                : "<auto>";

            return string.Format("[SubscriptionConfiguration QueueNames={0}]", queueNames);
        }
    }
}