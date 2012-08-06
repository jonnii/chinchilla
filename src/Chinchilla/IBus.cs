using System;

namespace Chinchilla
{
    /// <summary>
    /// An IBus is used to publish or subscribe to messages on a specific broker. It is
    /// responsible for managing the lifecycle of subscriptions and publish channels.
    /// </summary>
    public interface IBus : IPublisher, IDisposable
    {
        IPublishChannel CreatePublishChannel();

        ISubscription Subscribe<T>(Action<T> onMessage);

        ISubscription Subscribe<T>(Action<T> onMessage, Action<ISubscriptionConfiguration> configurator);
    }
}
