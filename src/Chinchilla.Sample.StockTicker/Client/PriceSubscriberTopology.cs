using Chinchilla.Topologies;
using Chinchilla.Topologies.Rabbit;

namespace Chinchilla.Sample.StockTicker.Client
{
    public class PriceSubscriberTopology : ISubscriptionTopology
    {
        private readonly Topology topology;

        public PriceSubscriberTopology(string clientId)
        {
            topology = new Topology();

            var clientExchangeName = string.Format("prices-{0}", clientId);
            var exchange = topology.DefineExchange(
                clientExchangeName,
                ExchangeType.Direct,
                isAutoDelete: true,
                durablility: Durability.Transient);

            Queue = topology.DefineQueue();
            Queue.BindTo(exchange);
        }

        public IQueue Queue { get; private set; }

        public void Visit(ITopologyVisitor visitor)
        {
            topology.Visit(visitor);
        }
    }
}