using Chinchilla.Topologies;
using Chinchilla.Topologies.Model;

namespace Chinchilla.Sample.SharedSubscriptions
{
    public class SubscriberTopology : IMessageTopologyBuilder
    {
        private readonly string routingKey;

        public SubscriberTopology(string routingKey)
        {
            this.routingKey = routingKey;
        }

        public IMessageTopology Build(IEndpoint endpoint)
        {
            var topology = new MessageTopology();

            var exchange = topology.DefineExchange(endpoint.MessageType, ExchangeType.Topic);
            topology.SubscribeQueue = topology.DefineQueue(endpoint.Name);

            if (endpoint.Ordinal == 0)
            {
                topology.SubscribeQueue.BindTo(exchange, routingKey);
            }

            topology.PublishTarget = exchange;

            return topology;
        }
    }
}
