using Chinchilla.Topologies.Model;

namespace Chinchilla.Topologies
{
    public class DefaultSubscribeTopologyBuilder : IMessageTopologyBuilder
    {
        private readonly string routingKey;

        public DefaultSubscribeTopologyBuilder() { }

        public DefaultSubscribeTopologyBuilder(string routingKey)
        {
            this.routingKey = routingKey;
        }

        public virtual IMessageTopology Build(IEndpoint endpoint)
        {
            var topology = new MessageTopology();

            topology.PublishExchange = topology.DefineExchange(endpoint.MessageType, ExchangeType.Topic);
            topology.SubscribeQueue = topology.DefineQueue(endpoint.Name);

            if (string.IsNullOrEmpty(routingKey))
            {
                topology.SubscribeQueue.BindTo(topology.PublishExchange);
            }
            else
            {
                topology.SubscribeQueue.BindTo(topology.PublishExchange, routingKey);
            }

            return topology;
        }
    }
}