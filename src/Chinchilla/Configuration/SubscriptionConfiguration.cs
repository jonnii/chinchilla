using System;
using System.Collections.Generic;
using System.Linq;
using Chinchilla.Topologies;

namespace Chinchilla.Configuration
{
    public class SubscriptionConfiguration : EndpointConfiguration, ISubscriptionConfiguration, ISubscriptionBuilder
    {
        private Func<IDeliveryProcessor, IDeliveryStrategy> strategyBuilder = handler => new TaskDeliveryStrategy();

        private Func<IBus, IFaultStrategy> failureStrategyBuilder = bus => new ErrorQueueFaultStrategy(bus);

        public SubscriptionConfiguration()
        {
            Name = Guid.NewGuid().ToString();
            PrefetchSize = 0;
            PrefetchCount = 50;
            MessageTopologyBuilder = new DefaultSubscribeTopologyBuilder();
            EndpointNames = Enumerable.Empty<string>();
        }

        public string Name { get; private set; }

        public uint PrefetchSize { get; private set; }

        public ushort PrefetchCount { get; private set; }

        public IEnumerable<string> EndpointNames { get; private set; }

        public ISubscriptionBuilder WithName(string name)
        {
            Name = name;
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

        public ISubscriptionBuilder SubscribeOn(string endpointName, params string[] otherEndpointNames)
        {
            EndpointNames = new[] { endpointName }.Concat(otherEndpointNames);

            return this;
        }

        public override string ToString()
        {
            var formattedNames = EndpointNames.Any()
                ? string.Join(",", EndpointNames)
                : "<auto>";

            return string.Format("[SubscriptionConfiguration EndpointNames={0}]", formattedNames);
        }
    }
}