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

        /// <summary>
        /// Overrides the default message serializer with the message serialize with the
        /// given content type
        /// </summary>
        IPublisherBuilder SerializeWith(string contentType);
    }
}