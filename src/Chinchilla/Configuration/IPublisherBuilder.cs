using System;
using Chinchilla.Topologies;

namespace Chinchilla.Configuration
{
    /// <summary>
    /// A publisher builder is used to configure the building of a publisher
    /// </summary>
    public interface IPublisherBuilder<TMessage>
    {
        /// <summary>
        /// Sets the topology on this publisher
        /// </summary>
        IPublisherBuilder<TMessage> SetTopology(IMessageTopologyBuilder messageTopologyBuilder);

        /// <summary>
        /// Sets the router
        /// </summary>
        IPublisherBuilder<TMessage> RouteWith<TRouter>()
            where TRouter : IRouter, new();

        /// <summary>
        /// Sets the router
        /// </summary>
        IPublisherBuilder<TMessage> RouteWith(IRouter router);

        /// <summary>
        /// Overrides the default endpoint name that the publisher will publish on
        /// </summary>
        IPublisherBuilder<TMessage> PublishOn(string endpointName);

        /// <summary>
        /// Overrides the default message serializer with the message serialize with the
        /// given content type
        /// </summary>
        IPublisherBuilder<TMessage> SerializeWith(string contentType);

        /// <summary>
        /// Sets the queue name to reply to
        /// </summary>
        IPublisherBuilder<TMessage> ReplyTo(string queueName);

        /// <summary>
        /// indicates whether or not the publisher should build the topology
        /// </summary>
        IPublisherBuilder<TMessage> BuildTopology(bool shouldBuildTopology);

        /// <summary>
        /// Indicates whether or not publisher confirms are enabled on this publisher
        /// </summary>
        IPublisherBuilder<TMessage> Confirm(bool shouldConfirm);

        /// <summary>
        /// Sets a custom publisher failure strategy which will be called when publishing
        /// a message fails
        /// </summary>
        IPublisherBuilder<TMessage> OnFailure<TStrategy>(params Action<TStrategy>[] configurations)
            where TStrategy : IPublisherFailureStrategy<TMessage>, new();

        /// <summary>
        /// Sets a custom publisher failure strategy instance which will be called when publishing
        /// a message fails
        /// </summary>
        IPublisherBuilder<TMessage> OnFailure<TStrategy>(TStrategy instance)
            where TStrategy : IPublisherFailureStrategy<TMessage>;
    }
}