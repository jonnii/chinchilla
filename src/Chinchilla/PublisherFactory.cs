using Chinchilla.Topologies.Rabbit;

namespace Chinchilla
{
    public class PublisherFactory : IPublisherFactory
    {
        private readonly IMessageSerializer messageSerializer;

        public PublisherFactory(IMessageSerializer messageSerializer)
        {
            this.messageSerializer = messageSerializer;
        }

        public IPublisher<TMessage> Create<TMessage>(
            IModelReference modelReference,
            IPublisherConfiguration configuration)
        {
            var topology = new Topology();
            var topologyBuilder = new TopologyBuilder(modelReference);

            var exchange = topology.DefineExchange(typeof(TMessage).Name, ExchangeType.Fanout);
            topologyBuilder.Visit(exchange);

            return new Publisher<TMessage>(
                modelReference,
                messageSerializer,
                exchange);
        }
    }
}