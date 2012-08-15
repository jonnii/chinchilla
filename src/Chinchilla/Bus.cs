using System;
using Chinchilla.Logging;
using Chinchilla.Topologies.Rabbit;

namespace Chinchilla
{
    public class Bus : IBus
    {
        private readonly ILogger logger = Logger.Create<Bus>();

        private readonly IModelFactory modelFactory;

        private readonly IMessageSerializer messageSerializer;

        private readonly ISubscriptionFactory subscriptionFactory;

        public Bus(
            IModelFactory modelFactory,
            IMessageSerializer messageSerializer,
            ISubscriptionFactory subscriptionFactory)
        {
            this.modelFactory = modelFactory;
            this.messageSerializer = messageSerializer;
            this.subscriptionFactory = subscriptionFactory;

            Topology = new Topology();
        }

        public ITopology Topology { get; private set; }

        public ISubscription Subscribe<T>(Action<T> onMessage)
        {
            return Subscribe(onMessage, SubscriptionConfiguration.Default);
        }

        public ISubscription Subscribe<T>(Action<T> onMessage, Action<ISubscriptionConfigurator> configurator)
        {
            var configuration = SubscriptionConfiguration.Default;

            configurator(configuration);

            return Subscribe(onMessage, configuration);
        }

        private ISubscription Subscribe<T>(Action<T> onMessage, SubscriptionConfiguration subscriptionConfiguration)
        {
            logger.DebugFormat("Subscribing to action callback of type {0}", typeof(T).Name);

            var model = modelFactory.CreateModel();
            var subscription = subscriptionFactory.Create(model, subscriptionConfiguration, onMessage);

            logger.DebugFormat("Starting subscription: {0}", subscription);
            subscription.Start();

            return subscription;
        }

        public ISubscription Subscribe<TMessage>(IConsumer<TMessage> consumer)
        {
            return Subscribe<TMessage>(consumer.Consume);
        }

        public IPublishChannel OpenPublishChannel()
        {
            return new PublishChannel(
                modelFactory.CreateModel(),
                messageSerializer);
        }

        public void Publish<T>(T message)
        {
            using (var publisher = OpenPublishChannel())
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