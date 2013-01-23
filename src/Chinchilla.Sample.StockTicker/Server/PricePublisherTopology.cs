using Chinchilla.Topologies;
using Chinchilla.Topologies.Model;

namespace Chinchilla.Sample.StockTicker.Server
{
    public class PricePublisherTopology : IMessageTopologyBuilder
    {
        public IMessageTopology Build(IEndpoint endpoint)
        {
            var messageTopology = new MessageTopology();
            messageTopology.PublishTarget = messageTopology.DefineExchange(endpoint.MessageType, ExchangeType.Topic);
            return messageTopology;
        }
    }
}