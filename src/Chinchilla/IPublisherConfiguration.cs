using Chinchilla.Topologies;

namespace Chinchilla
{
    public interface IPublisherConfiguration
    {
        IMessageTopology BuildTopology(IEndpoint endpoint);
    }
}