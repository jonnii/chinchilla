using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Chinchilla.Logging;
using Chinchilla.Topologies.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Chinchilla
{
    public class ModelReference : Trackable, IModelReference
    {
        private readonly ILogger logger = Logger.Create<ModelReference>();

        private readonly BlockingCollection<BasicDeliverEventArgs> deliverEventArgsQueue =
            new BlockingCollection<BasicDeliverEventArgs>();

        private readonly List<IQueue> consumerQueues = new List<IQueue>();

        private IModel model;

        public ModelReference(IModel model)
            : this(model, Guid.NewGuid().ToString())
        {

        }

        public ModelReference(IModel model, string tag)
        {
            this.model = model;

            Tag = tag;
        }

        public string Tag { get; private set; }

        public void Execute(Action<IModel> action)
        {
            action(model);
        }

        public TR Execute<TR>(Func<IModel, TR> func)
        {
            return func(model);
        }

        public void Reconnect(IModel newModel)
        {
            logger.DebugFormat("Reconnecting: {0}", Tag);

            model = newModel;

            foreach (var queue in consumerQueues)
            {
                BindConsumerToQueue(queue);
            }
        }

        public BlockingCollection<BasicDeliverEventArgs> GetConsumerQueue(IQueue queue)
        {
            consumerQueues.Add(queue);

            BindConsumerToQueue(queue);

            return deliverEventArgsQueue;
        }

        private void BindConsumerToQueue(IQueue queue)
        {
            var consumerTag = string.Format("{0}@{1}", Tag, queue.Name);

            var consumer = new SharedBlockingCollectionBasicConsumer(model, deliverEventArgsQueue)
            {
                ConsumerTag = consumerTag
            };

            model.BasicConsume(
                queue.Name,             // queue
                false,                  // noAck 
                consumer.ConsumerTag,   // consumerTag
                consumer);              // consumer
        }

        public override void Dispose()
        {
            deliverEventArgsQueue.CompleteAdding();

            if (model != null)
            {
                model.Close();
                model.Dispose();
            }

            base.Dispose();
        }
    }
}