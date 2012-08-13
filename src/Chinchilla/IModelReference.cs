using System;
using System.Collections.Concurrent;
using Chinchilla.Topologies.Rabbit;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Chinchilla
{
    /// <summary>
    /// A model reference is a reference to a RabbitMQ Model. The underlying model
    /// will be updated by a connection if the connection is closed.
    /// </summary>
    public interface IModelReference : IDisposable
    {
        /// <summary>
        /// Raised when this model reference is disposed
        /// </summary>
        event EventHandler<EventArgs> Disposed;

        /// <summary>
        /// Executes an action on the model
        /// </summary>
        void Execute(Action<IModel> action);

        /// <summary>
        /// Executes a function on the model, returning the result
        /// </summary>
        TR Execute<TR>(Func<IModel, TR> func);

        /// <summary>
        /// Gets a consumer queue
        /// </summary>
        /// <param name="queue">The queue to consume</param>
        /// <returns>A blocking collection of basic deliver event args</returns>
        BlockingCollection<BasicDeliverEventArgs> GetConsumerQueue(IQueue queue);

        /// <summary>
        /// Changes the underlying model of this model references, only to be used
        /// when a connection is reset
        /// </summary>
        void Reconnect(IModel newModel);
    }
}