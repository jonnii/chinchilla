using Chinchilla.Topologies;

namespace Chinchilla.Configuration
{
    public interface IPublisherBuilder
    {
        IPublisherBuilder SetTopology(IMessageTopologyBuilder messageTopologyBuilder);

        IPublisherBuilder RouteWith<TRouter>()
            where TRouter : IRouter, new();

        IPublisherBuilder RouteWith(IRouter router);
    }
}