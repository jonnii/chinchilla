using Chinchilla.Topologies;
using Chinchilla.Topologies.Rabbit;

namespace Chinchilla.Sample.StockTicker.Server
{
    public class PricePublisherTopology : IPublisherTopology
    {
        private readonly Topology topology;

        public PricePublisherTopology(string exchangeName)
        {
            topology = new Topology();
            Exchange = topology.DefineExchange(exchangeName, ExchangeType.Topic);
        }

        public IExchange Exchange { get; private set; }

        public void Visit(ITopologyVisitor visitor)
        {
            topology.Visit(visitor);
        }
    }
}