using System;
using Chinchilla.Configuration;

namespace Chinchilla
{
    /// <summary>
    /// An IBus is used to publish or subscribe to messages on a specific broker. It is
    /// responsible for managing the lifecycle of subscriptions and publish channels.
    /// </summary>
    public interface IBus : IDisposable
    {
        /// <summary>
        /// Creates a publisher for a specific message type
        /// </summary>
        IPublisher<TMessage> CreatePublisher<TMessage>();

        /// <summary>
        /// Creates a publisher for a specific message type with custom configuration
        /// </summary>
        IPublisher<TMessage> CreatePublisher<TMessage>(Action<IPublisherBuilder> builder);

        /// <summary>
        /// Publishes a message on the default publisher
        /// </summary>
        void Publish<TMessage>(TMessage message);

        ISubscription Subscribe<TMessage>(Action<TMessage> onMessage);

        ISubscription Subscribe<TMessage>(Action<TMessage> onMessage, Action<ISubscriptionBuilder> builder);

        ISubscription Subscribe<TMessage>(Action<TMessage, IDeliveryContext> onMessage);

        ISubscription Subscribe<TMessage>(Action<TMessage, IDeliveryContext> onMessage, Action<ISubscriptionBuilder> builder);

        ISubscription Subscribe(IConsumer consumer);

        ISubscription Subscribe<TConsumer>()
            where TConsumer : IConsumer;
    }
}
