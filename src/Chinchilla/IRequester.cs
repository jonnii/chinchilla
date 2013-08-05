using System;
using System.Threading.Tasks;

namespace Chinchilla
{
    public interface IRequester<in TRequest, TResponse> : IDisposable
    {
        /// <summary>
        /// Publishes a request message which expects a response
        /// </summary>
        /// <typeparam name="TResponse">The type of the response message</typeparam>
        void Request(TRequest message, Action<TResponse> onResponse);

        /// <summary>
        /// Publishes a request message which expects a response, returns a task which
        /// will be completed with the response
        /// </summary>
        /// <typeparam name="TResponse">The type of the response message</typeparam>
        /// <returns>A task for the response</returns>
        Task<TResponse> RequestAsync(TRequest message);
    }
}