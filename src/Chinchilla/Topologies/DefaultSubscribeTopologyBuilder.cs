using Chinchilla.Topologies.Model;

namespace Chinchilla.Topologies
{
    public class DefaultSubscribeTopologyBuilder : IMessageTopologyBuilder
    {
        public virtual IMessageTopology Build(IEndpoint endpoint)
        {
            var topology = new MessageTopology();

            var exchange = topology.DefineExchange(endpoint.MessageType, ExchangeType.Topic);

            topology.SubscribeQueue = topology.DefineQueue(endpoint.Name);
            topology.SubscribeQueue.BindTo(exchange);

            topology.PublishExchange = exchange;

            return topology;
        }
    }
}