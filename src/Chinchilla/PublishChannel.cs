using Chinchilla.Topologies.Rabbit;
using ExchangeType = Chinchilla.Topologies.Rabbit.ExchangeType;

namespace Chinchilla
{
    public class PublishChannel : IPublishChannel
    {
        private readonly IModelReference modelReference;

        private readonly IMessageSerializer serializer;

        private readonly Topology topology;

        private readonly TopologyBuilder topologyBuilder;

        private bool disposed;

        public PublishChannel(
            IModelReference modelReference,
            IMessageSerializer serializer)
        {
            this.modelReference = modelReference;
            this.serializer = serializer;

            topology = new Topology();
            topologyBuilder = new TopologyBuilder(modelReference);
        }

        public IExchange Exchange { get; set; }

        public long NumPublishedMessages { get; private set; }

        public void Publish<TMessage>(TMessage message)
        {
            Exchange = topology.DefineExchange(typeof(TMessage).Name, ExchangeType.Fanout);
            topologyBuilder.Visit(Exchange);

            var defaultProperties = modelReference.Execute(m => m.CreateBasicProperties());

            var serializedMessage = serializer.Serialize(
                Message.Create(message));

            modelReference.Execute(
                m => m.BasicPublish(
                    Exchange.Name,
                    "#",
                    defaultProperties,
                    serializedMessage));

            ++NumPublishedMessages;
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            modelReference.Dispose();

            disposed = true;
        }
    }
}