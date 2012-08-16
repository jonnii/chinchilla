using Chinchilla.Topologies;
using Chinchilla.Topologies.Model;

namespace Chinchilla.Sample.StockTicker.Client
{
    public class PriceSubscriberTopology : ISubscriberTopology
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

            SubscribeQueue = topology.DefineQueue();
            SubscribeQueue.BindTo(exchange);
        }

        public IQueue SubscribeQueue { get; private set; }

        public void Visit(ITopologyVisitor visitor)
        {
            topology.Visit(visitor);
        }
    }
}