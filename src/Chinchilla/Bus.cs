using System;
using Chinchilla.Logging;
using Chinchilla.Topologies.Rabbit;

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

            Topology = new Topology();
        }

        public ITopology Topology { get; private set; }

        public ISubscription Subscribe<TMessage>(Action<TMessage> onMessage)
        {
            return Subscribe(onMessage, SubscriptionConfiguration.Default);
        }

        public ISubscription Subscribe<TMessage>(Action<TMessage> onMessage, Action<ISubscriptionBuilder> builder)
        {
            var configuration = SubscriptionConfiguration.Default;

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
            return CreatePublisher<TMessage>(PublisherConfiguration.Default);
        }

        public IPublisher<TMessage> CreatePublisher<TMessage>(Action<IPublisherBuilder> builder)
        {
            var configuration = PublisherConfiguration.Default;

            builder(configuration);

            return CreatePublisher<TMessage>(configuration);
        }

        private IPublisher<TMessage> CreatePublisher<TMessage>(IPublisherConfiguration configuration)
        {
            logger.DebugFormat("creating publisher for {0}", typeof(TMessage).Name);

            var model = modelFactory.CreateModel();

            var publisher = publisherFactory.Create<TMessage>(model, configuration);
            publisher.Start();
            return publisher;
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