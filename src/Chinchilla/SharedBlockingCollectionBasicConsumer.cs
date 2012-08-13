using System;
using System.Collections.Concurrent;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Chinchilla
{
    public class SharedBlockingCollectionBasicConsumer : DefaultBasicConsumer
    {
        private readonly BlockingCollection<BasicDeliverEventArgs> sharedQueue;

        public SharedBlockingCollectionBasicConsumer(
            IModel model,
            BlockingCollection<BasicDeliverEventArgs> sharedQueue)
            : base(model)
        {
            this.sharedQueue = sharedQueue;
        }

        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, byte[] body)
        {
            try
            {
                sharedQueue.Add(new BasicDeliverEventArgs
                {
                    Exchange = exchange,
                    RoutingKey = routingKey,
                    ConsumerTag = consumerTag,
                    DeliveryTag = deliveryTag,
                    Redelivered = redelivered,
                    BasicProperties = properties,
                    Body = body
                });
            }
            catch (InvalidOperationException)
            {

            }
        }
    }
}