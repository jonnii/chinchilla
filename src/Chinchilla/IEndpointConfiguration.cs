using Chinchilla.Topologies;

namespace Chinchilla
{
    public interface IEndpointConfiguration
    {
        IMessageTopology BuildTopology(IEndpoint endpoint);
    }
}