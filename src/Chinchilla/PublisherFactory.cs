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
            var topology = configuration.BuildTopology<TMessage>();

            var topologyBuilder = new TopologyBuilder(modelReference);
            topology.Visit(topologyBuilder);

            return new Publisher<TMessage>(
                modelReference,
                messageSerializer,
                topology.Exchange);
        }
    }
}