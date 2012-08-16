using System;
using Chinchilla.Logging;

namespace Chinchilla
{
    public class Bus : IBus
    {
        private readonly ILogger logger = Logger.Create<Bus>();

        private readonly IModelFactory modelFactory;

        private readonly IPublisherFactory publisherFactory;

        private readonly ISubscriptionFactory subscriptionFactory;

        public Bus(
            IModelFactory modelFactory,
            IPublisherFactory publisherFactory,
            ISubscriptionFactory subscriptionFactory)
        {
            this.modelFactory = modelFactory;
            this.publisherFactory = publisherFactory;
            this.subscriptionFactory = subscriptionFactory;
        }

        public ISubscription Subscribe<TMessage>(Action<TMessage> onMessage)
        {
            return Subscribe(onMessage, new SubscriptionConfiguration());
        }

        public ISubscription Subscribe<TMessage>(Action<TMessage> onMessage, Action<ISubscriptionBuilder> builder)
        {
            var configuration = new SubscriptionConfiguration();

            builder(configuration);

            return Subscribe(onMessage, configuration);
        }

        public ISubscription Subscribe<TMessage>(IConsumer<TMessage> consumer)
        {
            return Subscribe<TMessage>(consumer.Consume);
        }

        private ISubscription Subscribe<TMessage>(Action<TMessage> onMessage, ISubscriptionConfiguration subscriptionConfiguration)
        {
            logger.DebugFormat("Subscribing to action callback of type {0}", typeof(TMessage).Name);

            var model = modelFactory.CreateModel();
            var subscription = subscriptionFactory.Create(model, subscriptionConfiguration, onMessage);

            logger.DebugFormat("Starting subscription: {0}", subscription);
            subscription.Start();

            return subscription;
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
            logger.DebugFormat("creating publisher for {0}", typeof(TMessage).Name);

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

        public void Dispose()
        {
            subscriptionFactory.Dispose();
            modelFactory.Dispose();
        }
    }
}