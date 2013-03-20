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

            topology.SubscribeQueue = topology.DefineQueue();
            topology.SubscribeQueue.BindTo(exchange);

            topology.PublishExchange = exchange;

            return topology;
        }
    }
}