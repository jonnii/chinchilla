using Chinchilla.Topologies;

namespace Chinchilla
{
    public interface IPublisherConfiguration
    {
        IPublisherTopology BuildTopology<TMessage>();
    }
}