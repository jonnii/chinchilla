using Chinchilla.Topologies.Model;

namespace Chinchilla.Topologies
{
    public abstract class SubscriptionTopology : ISubscriberTopology
    {
        private readonly Topology topology;

        protected SubscriptionTopology()
        {
            topology = new Topology();
        }

        public IQueue SubscribeQueue { get; protected set; }

        protected ITopology Topology
        {
            get { return topology; }
        }

        public void Visit(ITopologyVisitor visitor)
        {
            topology.Visit(visitor);
        }
    }
}