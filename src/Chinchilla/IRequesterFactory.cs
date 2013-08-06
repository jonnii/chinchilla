using System;

namespace Chinchilla
{
    /// <summary>
    /// The requester factory is responsible for creating and managing
    /// all the requesters
    /// </summary>
    public interface IRequesterFactory : IDisposable
    {
        /// <summary>
        /// Create a requester
        /// </summary>
        /// <typeparam name="TRequest">The request message type</typeparam>
        /// <typeparam name="TResponse">The response message type</typeparam>
        /// <param name="bus">The bus used to build the component parts for the requester</param>
        /// <returns>A requester</returns>
        IRequester<TRequest, TResponse> Create<TRequest, TResponse>(IBus bus)
            where TRequest : ICorrelated
            where TResponse : ICorrelated;
    }
}
