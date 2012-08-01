using System;
using Chinchilla.Topologies.Rabbit;
using RabbitMQ.Client;
using ExchangeType = Chinchilla.Topologies.Rabbit.ExchangeType;

namespace Chinchilla
{
    public class Bus : IBus
    {
        private readonly IConnection connection;

        private readonly IMessageSerializer messageSerializer;

        public Bus(
            IConnection connection,
            IMessageSerializer messageSerializer)
        {
            this.connection = connection;
            this.messageSerializer = messageSerializer;

            Topology = new Topology();
        }

        public ITopology Topology { get; private set; }

        public ISubscription Subscribe<T>(Action<T> onMessage)
        {
            var queue = Topology.DefineQueue();
            var exchange = Topology.DefineExchange(typeof(T).Name, ExchangeType.Fanout);

            queue.BindTo(exchange);

            return Subscribe(queue, exchange, onMessage);
        }

        public IPublishChannel CreatePublishChannel()
        {
            return new PublishChannel(
                connection.CreateModel(),
                messageSerializer);
        }

        private ISubscription Subscribe<T>(IQueue queue, IExchange exchange, Action<T> onMessage)
        {
            return new SubscriptionHandle(queue);
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