using Chinchilla.Topologies;

namespace Chinchilla.Configuration
{
    public interface IPublisherBuilder
    {
        IPublisherBuilder SetTopology(IMessageTopologyBuilder messageTopologyBuilder);

        IPublisherBuilder RouteWith<TRouter>()
            where TRouter : IRouter, new();

        IPublisherBuilder RouteWith(IRouter router);

        /// <summary>
        /// Overrides the default endpoint name that the publisher will publish on
        /// </summary>
        IPublisherBuilder PublishOn(string endpointName);
    }
}