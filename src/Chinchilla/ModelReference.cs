using System;
using System.Collections.Concurrent;
using Chinchilla.Topologies.Rabbit;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Chinchilla
{
    public class ModelReference : IModelReference
    {
        private readonly IModel model;

        private readonly BlockingCollection<BasicDeliverEventArgs> deliverEventArgsQueue =
            new BlockingCollection<BasicDeliverEventArgs>();

        public ModelReference(IModel model)
        {
            this.model = model;
        }

        public void Execute(Action<IModel> action)
        {
            action(model);
        }

        public TR Execute<TR>(Func<IModel, TR> func)
        {
            return func(model);
        }

        public BlockingCollection<BasicDeliverEventArgs> StartConsuming(IQueue queue)
        {
            var consumer = new SharedBlockingCollectionBasicConsumer(model, deliverEventArgsQueue)
            {
                ConsumerTag = Guid.NewGuid().ToString()
            };

            model.BasicConsume(
                queue.Name,             // queue
                false,                  // noAck 
                consumer.ConsumerTag,   // consumerTag
                consumer);              // consumer

            return deliverEventArgsQueue;
        }

        public void Dispose()
        {
            deliverEventArgsQueue.CompleteAdding();
            model.Close();
            model.Dispose();
        }
    }
}