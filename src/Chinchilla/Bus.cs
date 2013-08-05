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

        private readonly ISubscriptionFactory subscriptionFactory;

        private readonly IConsumerFactory consumerFactory;

        public Bus(
            IModelFactory modelFactory,
            IConsumerFactory consumerFactory,
            IPublisherFactory publisherFactory,
            ISubscriptionFactory subscriptionFactory)
        {
            this.modelFactory = modelFactory;
            this.publisherFactory = publisherFactory;
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
            return CreatePublisher<TMessage>(new PublisherConfiguration());
        }

        public IPublisher<TMessage> CreatePublisher<TMessage>(Action<IPublisherBuilder> builder)
        {
            var configuration = new PublisherConfiguration();

            builder(configuration);

            return CreatePublisher<TMessage>(configuration);
        }

        private IPublisher<TMessage> CreatePublisher<TMessage>(IPublisherConfiguration configuration)
        {
            logger.DebugFormat("Creating publisher for {0} with configuration {1}",
                typeof(TMessage).Name,
                configuration);

            var model = modelFactory.CreateModel();

            return publisherFactory.Create<TMessage>(model, configuration);
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
            logger.InfoFormat(
                "Creating Requester for {0} responding to {1}",
                typeof(TRequest).Name,
                typeof(TResponse).Name);

            var requester = new Requester<TRequest, TResponse>(this);
            requester.Start();
            return requester;
        }

        public void Request<TRequest, TResponse>(TRequest message, Action<TResponse> onResponse)
            where TRequest : ICorrelated
            where TResponse : ICorrelated
        {
            var requester = CreateRequester<TRequest, TResponse>();
            requester.Request(message, response =>
            {
                onResponse(response);
                requester.Dispose();
            });
        }

        public Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest message)
            where TRequest : ICorrelated
            where TResponse : ICorrelated
        {
            var requester = CreateRequester<TRequest, TResponse>();
            var task = requester.RequestAsync(message);

            task.ContinueWith(c => requester.Dispose());

            return task;
        }

        public void Dispose()
        {
            consumerFactory.Dispose();
            subscriptionFactory.Dispose();
            publisherFactory.Dispose();
            modelFactory.Dispose();
        }
    }
}