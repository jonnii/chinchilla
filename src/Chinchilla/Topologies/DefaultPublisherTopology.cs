using Chinchilla.Topologies.Rabbit;

namespace Chinchilla.Topologies
{
    public class DefaultPublisherTopology : IPublisherTopology
    {
        private readonly Topology topology;

        public DefaultPublisherTopology(string exchangeName)
        {
            topology = new Topology();
            Exchange = topology.DefineExchange(exchangeName, ExchangeType.Fanout);
        }

        public IExchange Exchange { get; private set; }

        public void Visit(ITopologyVisitor visitor)
        {
            topology.Visit(visitor);
        }
    }
}