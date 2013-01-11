using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Subcribes an action handler for a specific message type
        /// </summary>
        /// <typeparam name="TMessage">The type of message to subscribe to</typeparam>
        /// <param name="onMessage">The handler to invoke when a message is received</param>
        /// <returns>A subscription</returns>
        ISubscription Subscribe<TMessage>(Action<TMessage> onMessage);

        /// <summary>
        /// Subcribes an action handler for a specific message type with a subscription builder
        /// to customize the subscription
        /// </summary>
        /// <typeparam name="TMessage">The type of message to subscribe to</typeparam>
        /// <param name="onMessage">The handler to invoke when a message is received</param>
        /// <param name="builder">The builder configuration</param>
        /// <returns>A subscription</returns>
        ISubscription Subscribe<TMessage>(Action<TMessage> onMessage, Action<ISubscriptionBuilder> builder);

        /// <summary>
        /// Subcribes an action handler for a specific message type
        /// </summary>
        /// <typeparam name="TMessage">The type of message to subscribe to</typeparam>
        /// <param name="onMessage">The handler to invoke when a message is received</param>
        /// <returns>A subscription</returns>
        ISubscription Subscribe<TMessage>(Action<TMessage, IDeliveryContext> onMessage);

        /// <summary>
        /// Subcribes an action handler for a specific message type with a subscription builder
        /// to customize the subscription
        /// </summary>
        /// <typeparam name="TMessage">The type of message to subscribe to</typeparam>
        /// <param name="onMessage">The handler to invoke when a message is received</param>
        /// <param name="builder">The builder configuration</param>
        /// <returns>A subscription</returns>
        ISubscription Subscribe<TMessage>(Action<TMessage, IDeliveryContext> onMessage, Action<ISubscriptionBuilder> builder);

        /// <summary>
        /// Subscribes a consumer
        /// </summary>
        /// <param name="consumer">The consumer to subscribe</param>
        /// <returns>A subscription</returns>
        ISubscription Subscribe(IConsumer consumer);

        /// <summary>
        /// Subscribes a consumer
        /// </summary>
        /// <typeparam name="TConsumer">The type of the consumer to subscribe</typeparam>
        /// <returns>A subscription</returns>
        ISubscription Subscribe<TConsumer>()
            where TConsumer : IConsumer;

        /// <summary>
        /// Gets all the subscription state
        /// </summary>
        SubscriptionState[] GetState();
    }
}
