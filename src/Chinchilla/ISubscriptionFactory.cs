using System;
using System.Collections.Generic;
using Chinchilla.Configuration;

namespace Chinchilla
{
    /// <summary>
    /// The subscription factory is responsible for creating and managing
    /// all the available subscriptions
    /// </summary>
    public interface ISubscriptionFactory : IDisposable
    {
        /// <summary>
        /// Lists the subscriptions
        /// </summary>
        /// <returns>A list of the subscriptions</returns>
        IEnumerable<ISubscription> List();

        /// <summary>
        /// Create a subscription
        /// </summary>
        /// <typeparam name="TMessage">The message type to subscribe to</typeparam>
        /// <param name="bus">The bus that will own this subscription</param>
        /// <param name="configuration">A subscription configuration</param>
        /// <param name="callback">A callback to call when a message is received</param>
        /// <returns>A subscription</returns>
        ISubscription Create<TMessage>(
            IBus bus,
            ISubscriptionConfiguration configuration,
            Action<TMessage, IDeliveryContext> callback);

        /// <summary>
        /// Finds a subscription by name
        /// </summary>
        /// <param name="name">The name of the subscription to find</param>
        /// <returns>A subscription</returns>
        ISubscription FindByName(string name);
    }
}