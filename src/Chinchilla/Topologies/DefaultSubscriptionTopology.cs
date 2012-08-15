using Chinchilla.Topologies.Rabbit;

namespace Chinchilla.Topologies
{
    public class DefaultSubscriptionTopology : ISubscriptionTopology
    {
        private readonly Topology topology;

        public DefaultSubscriptionTopology(string messageType)
        {
            topology = new Topology();

            var exchange = topology.DefineExchange(messageType, ExchangeType.Fanout);

            Queue = topology.DefineQueue(messageType);
            Queue.BindTo(exchange);
        }

        public IQueue Queue { get; private set; }

        public void Visit(ITopologyVisitor visitor)
        {
            topology.Visit(visitor);
        }
    }
}