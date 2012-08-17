using System;
using Chinchilla.Topologies;

namespace Chinchilla
{
    public class PublisherConfiguration : IPublisherConfiguration, IPublisherBuilder
    {
        private Func<Endpoint, IPublisherTopology> publisherTopology;

        public PublisherConfiguration()
        {
            publisherTopology = endpoint => new DefaultTopology(endpoint);
        }

        public IPublisherBuilder SetTopology(Func<Endpoint, IPublisherTopology> customTopology)
        {
            publisherTopology = customTopology;
            return this;
        }

        public IPublisherTopology BuildTopology(Endpoint messageType)
        {
            return publisherTopology(messageType);
        }
    }
}