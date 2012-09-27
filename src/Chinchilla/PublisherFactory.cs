using Chinchilla.Configuration;
using Chinchilla.Topologies.Model;

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
            var messageType = typeof(TMessage).Name;
            var endpoint = new Endpoint(configuration.EndpointName ?? messageType, messageType);
            var topology = configuration.BuildTopology(endpoint);

            var topologyBuilder = new TopologyBuilder(modelReference);
            topology.Visit(topologyBuilder);

            var router = configuration.BuildRouter();

            return new Publisher<TMessage>(
                modelReference,
                messageSerializer,
                topology.PublishExchange,
                router);
        }
    }
}