using Chinchilla.Topologies;

namespace Chinchilla.Configuration
{
    public abstract class EndpointConfiguration : IEndpointConfiguration
    {
        public IMessageTopologyBuilder MessageTopologyBuilder { get; protected set; }

        public IMessageTopology BuildTopology(IEndpoint endpoint)
        {
            return MessageTopologyBuilder.Build(endpoint);
        }
    }
}