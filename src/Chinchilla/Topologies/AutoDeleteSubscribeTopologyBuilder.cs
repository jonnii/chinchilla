using Chinchilla.Topologies.Model;

namespace Chinchilla.Topologies
{
    public class AutoDeleteSubscribeTopologyBuilder : IMessageTopologyBuilder
    {
        public IMessageTopology Build(IEndpoint endpoint)
        {
            var topology = new MessageTopology();

            var exchange = topology.DefineExchange(endpoint.MessageType, ExchangeType.Topic);

            topology.SubscribeQueue = topology.DefineQueue();
            topology.SubscribeQueue.BindTo(exchange);

            return topology;
        }
    }
}