using Chinchilla.Topologies.Model;

namespace Chinchilla.Topologies
{
    public class MessageTopology : Topology, IMessageTopology
    {
        public IQueue SubscribeQueue { get; set; }

        public IBindable PublishTarget { get; set; }
    }
}