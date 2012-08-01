using System;
using Chinchilla.Topologies.Rabbit;
using RabbitMQ.Client;

namespace Chinchilla
{
    public class Bus : IBus
    {
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
            var model = connection.CreateModel();
            var subscription = subscriptionFactory.Create(model, onMessage);

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