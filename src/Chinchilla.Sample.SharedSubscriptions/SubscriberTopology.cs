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

            topology.PublishExchange = topology.DefineExchange(endpoint.MessageType, ExchangeType.Topic);

            topology.SubscribeQueue = topology.DefineQueue(endpoint.Name);
            topology.SubscribeQueue.BindTo(topology.PublishExchange, routingKey);

            return topology;
        }
    }
}
