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

            topology.SubscribeQueue = topology.DefineQueue("prices." + clientId);
            topology.SubscribeQueue.IsAutoDelete = true;
            topology.SubscribeQueue.Durability = Durability.Transient;

            return topology;
        }
    }
}