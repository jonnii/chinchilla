using System;
using Chinchilla.Topologies;

namespace Chinchilla
{
    public class PublisherConfiguration : IPublisherConfiguration, IPublisherBuilder
    {
        public PublisherConfiguration()
        {
            MessageTopologyBuilder = new DefaultMessageTopologyBuilder();
        }

        public IMessageTopologyBuilder MessageTopologyBuilder { get; private set; }

        public IPublisherBuilder SetTopology(IMessageTopologyBuilder messageTopologyBuilder)
        {
            MessageTopologyBuilder = messageTopologyBuilder;
            return this;
        }

        public IMessageTopology BuildTopology(IEndpoint endpoint)
        {
            return MessageTopologyBuilder.Build(endpoint);
        }
    }
}