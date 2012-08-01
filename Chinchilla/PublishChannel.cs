using System.Text;
using Chinchilla.Topologies.Rabbit;
using RabbitMQ.Client;
using ExchangeType = Chinchilla.Topologies.Rabbit.ExchangeType;

namespace Chinchilla
{
    public class PublishChannel : IPublishChannel
    {
        private readonly IModel model;

        private readonly IMessageSerializer serializer;

        private readonly Topology topology;

        private readonly TopologyBuilder topologyBuilder;

        private bool disposed;

        public PublishChannel(
            IModel model,
            IMessageSerializer serializer)
        {
            this.model = model;
            this.serializer = serializer;

            topology = new Topology();
            topologyBuilder = new TopologyBuilder(model);
        }

        public long PublishedMessages { get; private set; }

        public void Publish<T>(T message)
        {
            var exchange = topology.DefineExchange(typeof(T).Name, ExchangeType.Fanout);
            topologyBuilder.Visit(exchange);

            var defaultProperties = model.CreateBasicProperties();

            var serializedMessage = serializer.Serialize(
                Message.Create(message));

            model.BasicPublish(
                exchange.Name,
                "#",
                defaultProperties,
                serializedMessage);

            ++PublishedMessages;
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            model.Abort();
            model.Dispose();

            disposed = true;
        }
    }
}