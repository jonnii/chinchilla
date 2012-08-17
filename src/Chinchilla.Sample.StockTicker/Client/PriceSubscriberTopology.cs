using Chinchilla.Topologies;
using Chinchilla.Topologies.Model;

namespace Chinchilla.Sample.StockTicker.Client
{
    public class PriceSubscriberTopology : IMessageTopologyBuilder
    {
        private readonly string clientId;

        public PriceSubscriberTopology(string clientId)
        {
            this.clientId = clientId;
        }

        public IMessageTopology Build(IEndpoint endpoint)
        {
            var topology = new MessageTopology();

            var clientExchangeName = string.Format("prices-{0}", clientId);
            var exchange = topology.DefineExchange(
                clientExchangeName,
                ExchangeType.Topic,
                isAutoDelete: true,
                durablility: Durability.Transient);

            topology.SubscribeQueue = topology.DefineQueue();
            topology.SubscribeQueue.BindTo(exchange);

            return topology;
        }
    }
}