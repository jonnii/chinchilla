using Chinchilla.Topologies;
using Chinchilla.Topologies.Model;

namespace Chinchilla.Sample.SharedSubscriptions
{
    public class MessagePublisherTopology : IMessageTopologyBuilder
    {
        public IMessageTopology Build(IEndpoint endpoint)
        {
            var topology = new MessageTopology();
            topology.PublishExchange = topology.DefineExchange(endpoint.Name, ExchangeType.Topic);

            return topology;
        }
    }
}