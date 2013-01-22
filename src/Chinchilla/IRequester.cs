using System;

namespace Chinchilla
{
    public interface IRequester<in TRequest, out TResponse> : IDisposable
    {
        /// <summary>
        /// Publishes a request message which expects a response
        /// </summary>
        /// <typeparam name="TResponse">The type of the response message</typeparam>
        void Request(TRequest message, Action<TResponse> onResponse);
    }
}