using System;
using Chinchilla.Topologies;

namespace Chinchilla
{
    public class PublisherConfiguration : IPublisherConfiguration, IPublisherBuilder
    {
        private Func<string, IPublisherTopology> publisherTopology;

        public PublisherConfiguration()
        {
            publisherTopology = messageType => new DefaultPublisherTopology(messageType);
        }

        public IPublisherBuilder SetTopology(Func<string, IPublisherTopology> customTopology)
        {
            publisherTopology = customTopology;
            return this;
        }

        public IPublisherTopology BuildTopology(string messageType)
        {
            return publisherTopology(messageType);
        }
    }
}