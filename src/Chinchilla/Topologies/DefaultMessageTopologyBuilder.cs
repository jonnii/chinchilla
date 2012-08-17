using Chinchilla.Topologies.Model;

namespace Chinchilla.Topologies
{
    public class DefaultMessageTopologyBuilder : IMessageTopologyBuilder
    {
        public IMessageTopology Build(IEndpoint endpoint)
        {
            var topology = new MessageTopology();

            topology.PublishExchange = topology.DefineExchange(endpoint.MessageType, ExchangeType.Topic);

            topology.SubscribeQueue = topology.DefineQueue(endpoint.Name);
            topology.SubscribeQueue.BindTo(topology.PublishExchange);

            return topology;
        }
    }
}