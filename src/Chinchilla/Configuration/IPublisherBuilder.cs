using Chinchilla.Topologies;

namespace Chinchilla.Configuration
{
    /// <summary>
    /// A publisher builder is used to configure the building of a publisher
    /// </summary>
    public interface IPublisherBuilder
    {
        /// <summary>
        /// Sets the topology on this publisher
        /// </summary>
        IPublisherBuilder SetTopology(IMessageTopologyBuilder messageTopologyBuilder);

        /// <summary>
        /// Sets the router
        /// </summary>
        IPublisherBuilder RouteWith<TRouter>()
            where TRouter : IRouter, new();

        /// <summary>
        /// Sets the router
        /// </summary>
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

        /// <summary>
        /// Sets the queue name to reply to
        /// </summary>
        IPublisherBuilder ReplyTo(string queueName);

        /// <summary>
        /// indicates whether or not the publisher should build the topology
        /// </summary>
        IPublisherBuilder BuildTopology(bool shouldBuildTopology);

        /// <summary>
        /// Indicates whether or not publisher confirms are enabled on this publisher
        /// </summary>
        IPublisherBuilder Confirm(bool shouldConfirm);
    }
}