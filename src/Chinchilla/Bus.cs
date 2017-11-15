using System;
using System.Linq;
using System.Threading.Tasks;
using Chinchilla.Configuration;
using Chinchilla.Logging;

namespace Chinchilla
{
    public class Bus : IBus
    {
        private readonly ILogger logger = Logger.Create<Bus>();

        private readonly IModelFactory modelFactory;

        private readonly IPublisherFactory publisherFactory;

        private readonly IRequesterFactory requesterFactory;

        private readonly ISubscriptionFactory subscriptionFactory;

        private readonly IConsumerFactory consumerFactory;

        public Bus(
            IModelFactory modelFactory,
            IConsumerFactory consumerFactory,
            IRequesterFactory requesterFactory,
            IPublisherFactory publisherFactory,
            ISubscriptionFactory subscriptionFactory)
        {
            this.modelFactory = modelFactory;
            this.publisherFactory = publisherFactory;
            this.requesterFactory = requesterFactory;
            this.subscriptionFactory = subscriptionFactory;
            this.consumerFactory = consumerFactory;
        }

        public ISubscription Subscribe<TMessage>(Action<TMessage> onMessage)
        {
            return Subscribe<TMessage>((m, c) => onMessage(m));
        }

        public ISubscription Subscribe<TMessage>(Action<TMessage> onMessage, Action<ISubscriptionBuilder> builder)
        {
            return Subscribe<TMessage>((m, c) => onMessage(m), builder);
        }

        public ISubscription Subscribe<TMessage>(Action<TMessage, IDeliveryContext> onMessage)
        {
            return Subscribe(onMessage, new SubscriptionConfiguration());
        }

        public ISubscription Subscribe<TMessage>(Action<TMessage, IDeliveryContext> onMessage, Action<ISubscriptionBuilder> builder)
        {
            var configuration = new SubscriptionConfiguration();

            builder(configuration);

            return Subscribe(onMessage, configuration);
        }

        private ISubscription Subscribe<TMessage>(Action<TMessage, IDeliveryContext> onMessage, ISubscriptionConfiguration subscriptionConfiguration)
        {
            logger.DebugFormat("Subscribing to action callback of type {0}", typeof(TMessage).Name);

            var subscription = subscriptionFactory.Create(
                this,
                subscriptionConfiguration,
                onMessage);

            if (subscription.IsStartable)
            {
                subscription.Start();
            }
            else
            {
                logger.InfoFormat("Could not start subscription, invalid configuration: {0}", subscription);
            }

            return subscription;
        }

        public ISubscription Subscribe<TConsumer>()
            where TConsumer : IConsumer
        {
            logger.DebugFormat("Buliding consumer {0}", typeof(TConsumer).Name);
            var consumer = consumerFactory.Build<TConsumer>();
            return Subscribe(consumer);
        }

        public SubscriptionState[] GetState()
        {
            return subscriptionFactory
                .List()
                .Select(s => s.State)
                .ToArray();
        }

        public ISubscription FindSubscription(string subscriptionName)
        {
            return subscriptionFactory.FindByName(subscriptionName);
        }

        public ISubscription Subscribe(IConsumer consumer)
        {
            logger.DebugFormat("Registering consumer {0}", consumer.GetType().Name);
            var instance = new ConsumerSubscriber(this, consumer);
            return instance.Connect();
        }

        public IPublisher<TMessage> CreatePublisher<TMessage>()
        {
            return CreatePublisher(new PublisherConfiguration<TMessage>());
        }

        public IPublisher<TMessage> CreatePublisher<TMessage>(Action<IPublisherBuilder<TMessage>> builder)
        {
            var configuration = new PublisherConfiguration<TMessage>();

            builder(configuration);

            return CreatePublisher(configuration);
        }

        private IPublisher<TMessage> CreatePublisher<TMessage>(IPublisherConfiguration<TMessage> configuration)
        {
            logger.DebugFormat("Creating publisher for {0} with configuration {1}",
                typeof(TMessage).Name,
                configuration);

            var model = modelFactory.CreateModel();

            return publisherFactory.Create(model, configuration);
        }

        public void Publish<TMessage>(TMessage message)
        {
            using (var publisher = CreatePublisher<TMessage>())
            {
                publisher.Publish(message);
            }
        }

        public IRequester<TRequest, TResponse> CreateRequester<TRequest, TResponse>()
            where TRequest : ICorrelated
            where TResponse : ICorrelated
        {
            return requesterFactory.Create<TRequest, TResponse>(this);
        }

        public void Request<TRequest, TResponse>(TRequest message, Action<TResponse> onResponse)
            where TRequest : ICorrelated
            where TResponse : ICorrelated
        {
            var requester = CreateRequester<TRequest, TResponse>();

            requester.Request(message, (response, context) =>
            {
                onResponse(response);

                // Because this is a single use requester we need to dispose of it somehow. Calling dispose
                // on the requester directly in the callback causes an exception (and ultimately a message on 
                // the error queue) because the message cannot be acked (since the queues will be disposed).
                // Use a delivery listener to schedule dispose for _after_ the delivery being delivered, that
                // way there's no issue with things already being disposed
                context.Delivery.RegisterDeliveryListener(new ActionDeliveryListener(requester.Dispose));
            });
        }

        public Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest message)
            where TRequest : ICorrelated
            where TResponse : ICorrelated
        {
            // We could call the async request method on the requester directly, but reuse the
            // above logic because a continue with on the async result _could_ be called in the same
            // context as the response being resolved which would cause the same object disposed exception
            // as above.

            var source = new TaskCompletionSource<TResponse>();
            Request<TRequest, TResponse>(message, t => Task.Run(() => source.SetResult(t)));
            return source.Task;
        }

        public void Dispose()
        {
            consumerFactory.Dispose();
            requesterFactory.Dispose();

            subscriptionFactory.Dispose();
            publisherFactory.Dispose();

            modelFactory.Dispose();
        }
    }
}