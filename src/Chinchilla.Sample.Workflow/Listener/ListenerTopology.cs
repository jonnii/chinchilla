using Chinchilla.Topologies;
using Chinchilla.Topologies.Model;

namespace Chinchilla.Sample.Workflow.Listener
{
    public class ListenerTopology : IMessageTopologyBuilder
    {
        public IMessageTopology Build(IEndpoint endpoint)
        {
            var topology = new MessageTopology();

            var exchange = topology.DefineExchange(endpoint.MessageType, ExchangeType.Topic);

            topology.SubscribeQueue = topology.DefineQueue();
            topology.SubscribeQueue.BindTo(exchange);

            topology.PublishExchange = exchange;

            return topology;
        }
    }
}