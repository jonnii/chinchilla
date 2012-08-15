using Chinchilla.Topologies.Rabbit;

namespace Chinchilla.Topologies
{
    public class DefaultSubscriptionTopology : ISubscriptionTopology
    {
        private readonly Topology topology;

        public DefaultSubscriptionTopology(string exchangeName)
        {
            topology = new Topology();

            var exchange = topology.DefineExchange(exchangeName, ExchangeType.Fanout);

            Queue = topology.DefineQueue(exchangeName);
            Queue.BindTo(exchange);
        }

        public IQueue Queue { get; private set; }

        public void Visit(ITopologyVisitor visitor)
        {
            topology.Visit(visitor);
        }
    }
}