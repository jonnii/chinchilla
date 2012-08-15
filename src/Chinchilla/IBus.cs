using System;

namespace Chinchilla
{
    /// <summary>
    /// An IBus is used to publish or subscribe to messages on a specific broker. It is
    /// responsible for managing the lifecycle of subscriptions and publish channels.
    /// </summary>
    public interface IBus : IPublisher, IDisposable
    {
        /// <summary>
        /// Opens a publish channel
        /// </summary>
        IPublishChannel OpenPublishChannel();

        ISubscription Subscribe<T>(Action<T> onMessage);

        ISubscription Subscribe<T>(Action<T> onMessage, Action<ISubscriptionConfigurator> configurator);

        ISubscription Subscribe<TMessage>(IConsumer<TMessage> consumer);
    }
}
