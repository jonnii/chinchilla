using System;
using System.Threading;
using Chinchilla.Topologies.Rabbit;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ExchangeType = Chinchilla.Topologies.Rabbit.ExchangeType;

namespace Chinchilla
{
    public class Subscription<T> : ISubscription
    {
        private readonly IModel model;

        private readonly IMessageSerializer messageSerializer;

        private readonly IMessageHandler<T> messageHandler;

        private readonly Topology topology;

        private readonly TopologyBuilder topologyBuilder;

        public Subscription(
            IModel model,
            IMessageSerializer messageSerializer,
            IMessageHandler<T> messageHandler)
        {
            this.model = model;
            this.messageSerializer = messageSerializer;
            this.messageHandler = messageHandler;

            topology = new Topology();
            topologyBuilder = new TopologyBuilder(model);
        }

        public void Start()
        {
            var queue = topology.DefineQueue(typeof(T).Name);
            var exchange = topology.DefineExchange(typeof(T).Name, ExchangeType.Fanout);

            queue.BindTo(exchange);

            topology.Visit(topologyBuilder);

            var consumer = new QueueingBasicConsumer(model)
            {
                ConsumerTag = Guid.NewGuid().ToString()
            };

            model.BasicConsume(
                queue.Name,             // queue
                false,                  // noAck 
                consumer.ConsumerTag,   // consumerTag
                consumer);              // consumer

            var start = new Thread(() =>
            {
                while (true)
                {
                    var item = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                    var message = messageSerializer.Deserialize<T>(item.Body);

                    messageHandler.Handle(message);

                    model.BasicAck(item.DeliveryTag, false);
                }
            });

            start.Start();
        }

        public void Dispose()
        {
            model.Dispose();
        }
    }
}
