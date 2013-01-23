using Chinchilla.Topologies.Model;

namespace Chinchilla.Topologies
{
    public class DefaultPublishTopologyBuilder : IMessageTopologyBuilder
    {
        public virtual IMessageTopology Build(IEndpoint endpoint)
        {
            var topology = new MessageTopology();

            topology.PublishTarget = topology.DefineExchange(endpoint.Name, ExchangeType.Topic);

            return topology;
        }
    }
}