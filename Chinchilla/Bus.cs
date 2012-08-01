using System;
using Chinchilla.Logging;
using Chinchilla.Topologies.Rabbit;
using RabbitMQ.Client;

namespace Chinchilla
{
    public class Bus : IBus
    {
        private readonly ILogger logger = Logger.Create<Bus>();

        private readonly IConnection connection;

        private readonly IMessageSerializer messageSerializer;

        private readonly ISubscriptionFactory subscriptionFactory;

        public Bus(
            IConnection connection,
            IMessageSerializer messageSerializer,
            ISubscriptionFactory subscriptionFactory)
        {
            this.connection = connection;
            this.messageSerializer = messageSerializer;
            this.subscriptionFactory = subscriptionFactory;

            Topology = new Topology();
        }

        public ITopology Topology { get; private set; }

        public ISubscription Subscribe<T>(Action<T> onMessage)
        {
            logger.DebugFormat("Subscribing to action callback of type {0}", typeof(T).Name);

            var model = connection.CreateModel();
            var subscription = subscriptionFactory.Create(model, onMessage);

            logger.DebugFormat("Starting subscription: {0}", subscription);
            subscription.Start();

            return subscription;
        }

        public IPublishChannel CreatePublishChannel()
        {
            return new PublishChannel(
                connection.CreateModel(),
                messageSerializer);
        }

        public void Publish<T>(T message)
        {
            var publisher = CreatePublishChannel();
            publisher.Publish(message);
        }

        public void Dispose()
        {

        }
    }
}