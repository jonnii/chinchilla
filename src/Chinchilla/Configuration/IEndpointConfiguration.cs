using Chinchilla.Topologies;

namespace Chinchilla.Configuration
{
    public interface IEndpointConfiguration
    {
        IMessageTopology BuildTopology(IEndpoint endpoint);
    }
}