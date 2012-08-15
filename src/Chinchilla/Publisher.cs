using Chinchilla.Topologies.Rabbit;
using ExchangeType = Chinchilla.Topologies.Rabbit.ExchangeType;

namespace Chinchilla
{
    public class Publisher<TMessage> : IPublisher<TMessage>
    {
        private readonly IModelReference modelReference;

        private readonly IMessageSerializer serializer;

        private readonly Topology topology;

        private readonly TopologyBuilder topologyBuilder;

        private bool disposed;

        public Publisher(
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

        public void Start()
        {
            
        }

        public void Publish(TMessage message)
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