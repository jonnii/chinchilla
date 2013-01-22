using System;

namespace Chinchilla
{
    public class Requester<TRequest, TResponse> : IRequester<TRequest, TResponse>
    {
        private readonly IPublisher<TRequest> publisher;

        public Requester(IPublisher<TRequest> publisher)
        {
            this.publisher = publisher;
        }

        public void Request(TRequest message, Action<TResponse> onResponse)
        {
            publisher.Publish(message);
        }

        private void HandleResponse(TResponse response)
        {

        }

        public void Dispose()
        {
        }
    }
}