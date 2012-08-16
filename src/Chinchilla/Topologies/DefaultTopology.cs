using Chinchilla.Topologies.Model;

namespace Chinchilla.Topologies
{
    public class DefaultTopology : Topology, ISubscriberTopology, IPublisherTopology
    {
        public DefaultTopology(string messageType)
        {
            PublishExchange = DefineExchange(messageType, ExchangeType.Fanout);

            SubscribeQueue = DefineQueue(messageType);
            SubscribeQueue.BindTo(PublishExchange);
        }

        public IQueue SubscribeQueue { get; private set; }

        public IExchange PublishExchange { get; private set; }
    }
}