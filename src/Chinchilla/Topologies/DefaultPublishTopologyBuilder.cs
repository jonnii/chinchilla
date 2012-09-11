using Chinchilla.Topologies.Model;

namespace Chinchilla.Topologies
{
    public class DefaultPublishTopologyBuilder : IMessageTopologyBuilder
    {
        public virtual IMessageTopology Build(IEndpoint endpoint)
        {
            var topology = new MessageTopology();

            topology.PublishExchange = topology.DefineExchange(endpoint.MessageType, ExchangeType.Topic);

            return topology;
        }
    }
}