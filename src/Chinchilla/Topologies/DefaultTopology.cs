using Chinchilla.Topologies.Model;

namespace Chinchilla.Topologies
{
    public class DefaultTopology : Topology, ISubscriberTopology, IPublisherTopology
    {
        public DefaultTopology(Endpoint endpoint)
        {
            PublishExchange = DefineExchange(endpoint.MessageType, ExchangeType.Topic);

            SubscribeQueue = DefineQueue(endpoint.Name);
            SubscribeQueue.BindTo(PublishExchange);
        }

        public IQueue SubscribeQueue { get; private set; }

        public IExchange PublishExchange { get; private set; }
    }
}