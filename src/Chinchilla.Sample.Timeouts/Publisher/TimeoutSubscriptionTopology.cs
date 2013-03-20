using System;
using Chinchilla.Topologies;
using Chinchilla.Topologies.Model;

namespace Chinchilla.Sample.Timeouts.Publisher
{
    public class TimeoutSubscriptionTopology : IMessageTopologyBuilder
    {
        public IMessageTopology Build(IEndpoint endpoint)
        {
            var topology = new MessageTopology();

            var exchange = topology.DefineExchange("messages.timedout", ExchangeType.Topic);

            var queueName = "Timeouts_" + Guid.NewGuid();

            topology.SubscribeQueue = topology.DefineQueue(queueName);
            topology.SubscribeQueue.IsAutoDelete = true;
            topology.SubscribeQueue.BindTo(exchange, queueName);

            topology.PublishExchange = exchange;

            return topology;
        }
    }
}