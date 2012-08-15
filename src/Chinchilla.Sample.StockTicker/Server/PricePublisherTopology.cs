using Chinchilla.Topologies;
using Chinchilla.Topologies.Rabbit;

namespace Chinchilla.Sample.StockTicker.Server
{
    public class PricePublisherTopology : IPublisherTopology
    {
        public IExchange Exchange { get; private set; }

        public void Visit(ITopologyVisitor visitor)
        {
            throw new System.NotImplementedException();
        }
    }
}