using Chinchilla.Topologies.Model;

namespace Chinchilla.Topologies
{
    public class DefaultResponseTopologyBuilder : IMessageTopologyBuilder
    {
        public IMessageTopology Build(IEndpoint endpoint)
        {
            var topology = new MessageTopology();
            topology.PublishExchange = topology.DefineExchange(string.Empty, ExchangeType.Direct);
            return topology;
        }
    }
}
