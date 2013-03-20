using Chinchilla.Topologies;
using Chinchilla.Topologies.Model;

namespace Chinchilla.Sample.Timeouts.Consumer
{
    public class DeadLetterTopology : IMessageTopologyBuilder
    {
        public IMessageTopology Build(IEndpoint endpoint)
        {
            var topology = new MessageTopology();

            var exchange = topology.DefineExchange(endpoint.MessageType, ExchangeType.Topic);

            // define an additional messages timedout topic exchange 
            topology.DefineExchange("messages.timedout", ExchangeType.Topic);
            topology.SubscribeQueue = topology.DefineQueue(endpoint.Name);

            // on the subscription queue we need to specify what the dead letter exchange name is
            topology.SubscribeQueue.DeadLetterExchange = "messages.timedout";
            topology.SubscribeQueue.BindTo(exchange);

            topology.PublishExchange = exchange;

            return topology;
        }
    }
}