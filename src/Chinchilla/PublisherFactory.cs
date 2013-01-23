using Chinchilla.Configuration;
using Chinchilla.Topologies.Model;

namespace Chinchilla
{
    public class PublisherFactory : TrackableFactory<Publisher>, IPublisherFactory
    {
        private readonly IMessageSerializers messageSerializers;

        public PublisherFactory(IMessageSerializers messageSerializers)
        {
            this.messageSerializers = messageSerializers;
        }

        public IPublisher<TMessage> Create<TMessage>(
            IModelReference modelReference,
            IPublisherConfiguration configuration)
        {
            var messageType = typeof(TMessage).Name;
            var endpoint = new Endpoint(configuration.EndpointName ?? messageType, messageType, 0);
            var topology = configuration.BuildTopology(endpoint);

            if (configuration.ShouldBuildTopology)
            {
                var topologyBuilder = new TopologyBuilder(modelReference);
                topology.Visit(topologyBuilder);
            }

            var router = configuration.BuildRouter();

            var messageSerializer = messageSerializers.FindOrDefault(
                configuration.ContentType);

            var publisher = new Publisher<TMessage>(
                modelReference,
                messageSerializer,
                topology.PublishExchange,
                router);

            Track(publisher);

            return publisher;
        }
    }
}