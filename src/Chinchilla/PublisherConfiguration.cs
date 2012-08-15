using Chinchilla.Topologies;

namespace Chinchilla
{
    public class PublisherConfiguration : IPublisherConfiguration, IPublisherBuilder
    {
        public static PublisherConfiguration Default
        {
            get { return new PublisherConfiguration(); }
        }

        public IPublisherBuilder SetTopology(IPublisherTopology publisherTopology)
        {
            return this;
        }
    }
}