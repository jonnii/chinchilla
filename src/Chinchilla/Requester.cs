using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Chinchilla.Logging;
using Chinchilla.Topologies;

namespace Chinchilla
{
    public class Requester<TRequest, TResponse> : IRequester<TRequest, TResponse>
        where TRequest : ICorrelated
        where TResponse : ICorrelated
    {
        private readonly ILogger logger = Logger.Create<Requester<TRequest, TResponse>>();

        private readonly IBus bus;

        private readonly ConcurrentDictionary<string, Action<TResponse, IDeliveryContext>> responders =
            new ConcurrentDictionary<string, Action<TResponse, IDeliveryContext>>();

        private IPublisher<TRequest> publisher;

        private ISubscription subscription;

        public Requester(IBus bus)
        {
            this.bus = bus;
        }

        public void Start()
        {
            subscription = bus.Subscribe<TResponse>(
                DispatchToRegisteredResponseHandler,
                    b => b.SetTopology<DefaultRequestTopologyBuilder>()
                          .DeliverUsing<TaskDeliveryStrategy>());

            var queue = subscription.Queues.First();
            publisher = bus.CreatePublisher<TRequest>(p => p.ReplyTo(queue.Name));
        }

        public void Request(TRequest message, Action<TResponse> onResponse)
        {
            Request(message, (response, context) => onResponse(response));
        }

        public void Request(TRequest message, Action<TResponse, IDeliveryContext> onResponse)
        {
            var correlationId = message.CorrelationId.ToString();

            if (RegisterResponseHandler(correlationId, onResponse))
            {
                publisher.Publish(message);
            }
        }

        public Task<TResponse> RequestAsync(TRequest message)
        {
            var source = new TaskCompletionSource<TResponse>();
            Request(message, source.SetResult);
            return source.Task;
        }

        public bool RegisterResponseHandler(string correlationId, Action<TResponse, IDeliveryContext> onResponse)
        {
            if (string.IsNullOrEmpty(correlationId))
            {
                var message = string.Format(
                   "The requester {0}/{1} tried to register a response handler for a message with no correlation id",
                   typeof(TRequest).Name,
                   typeof(TResponse).Name);

                throw new ChinchillaException(message);
            }

            return responders.TryAdd(correlationId, onResponse);
        }

        public void DispatchToRegisteredResponseHandler(TResponse response, IDeliveryContext deliveryContext)
        {
            var correlationId = deliveryContext.Delivery.CorrelationId;

            if (string.IsNullOrEmpty(correlationId))
            {
                var message = string.Format(
                    "The requester {0}/{1} received a response with with no correlation id",
                    typeof(TRequest).Name,
                    typeof(TResponse).Name);

                throw new ChinchillaException(message);
            }

            Action<TResponse, IDeliveryContext> handler;
            if (!responders.TryGetValue(correlationId, out handler))
            {
                var message = string.Format(
                    "The requester {0}/{1} received a response with a correlation id that wasn't found " +
                    "in the responders dictionary.",
                    typeof(TRequest).Name,
                    typeof(TResponse).Name);

                throw new ChinchillaException(message);
            }

            handler(response, deliveryContext);
        }

        public void Dispose()
        {
            logger.Debug("Disposing of requester");

            if (publisher != null)
            {
                logger.Debug(" -> Disposing of publisher");
                publisher.Dispose();
            }

            if (subscription != null)
            {
                logger.Debug(" -> Disposing of subscription");
                subscription.Dispose();
            }
        }
    }
}