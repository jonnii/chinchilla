using System;

namespace Chinchilla
{
    /// <summary>
    /// An IBus is used to publish or subscribe to messages on a specific broker. It is
    /// responsible for managing the lifecycle of subscriptions and publish channels.
    /// </summary>
    public interface IBus : IDisposable
    {
        /// <summary>
        /// Opens a publish channel
        /// </summary>
        IPublisher<TMessage> CreatePublisher<TMessage>();

        IPublisher<TMessage> CreatePublisher<TMessage>(Action<IPublisherBuilder> builder);

        void Publish<TMessage>(TMessage message);

        ISubscription Subscribe<TMessage>(Action<TMessage> onMessage);

        ISubscription Subscribe<TMessage>(Action<TMessage> onMessage, Action<ISubscriptionBuilder> builder);

        ISubscription Subscribe<TMessage>(IConsumer<TMessage> consumer);
    }
}
