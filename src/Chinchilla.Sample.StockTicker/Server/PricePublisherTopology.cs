using Chinchilla.Topologies;
using Chinchilla.Topologies.Model;

namespace Chinchilla.Sample.StockTicker.Server
{
    public class PricePublisherTopology : Topology, ISubscriberTopology, IPublisherTopology
    {
        public PricePublisherTopology(string exchangeName)
        {
            PublishExchange = DefineExchange(exchangeName, ExchangeType.Topic);
        }

        public IExchange PublishExchange { get; private set; }

        public IQueue SubscribeQueue { get; private set; }
    }
}