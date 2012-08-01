using System.Text;
using Chinchilla.Topologies.Rabbit;
using RabbitMQ.Client;
using ExchangeType = Chinchilla.Topologies.Rabbit.ExchangeType;

namespace Chinchilla
{
    public class PublishChannel : IPublishChannel
    {
        private readonly IModel model;

        private readonly Topology topology;

        private readonly TopologyBuilder topologyBuilder;

        private bool disposed;

        public PublishChannel(IModel model)
        {
            this.model = model;

            topology = new Topology();
            topologyBuilder = new TopologyBuilder(model);
        }

        public long PublishedMessages { get; private set; }

        public void Publish<T>(T message)
        {
            var exchange = topology.DefineExchange(typeof(T).Name, ExchangeType.Fanout);
            topologyBuilder.Visit(exchange);

            var defaultProperties = model.CreateBasicProperties();

            model.BasicPublish(
                exchange.Name,
                "#",
                defaultProperties,
                Encoding.Default.GetBytes("Body"));

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