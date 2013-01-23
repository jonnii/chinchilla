using Chinchilla.Topologies.Model;

namespace Chinchilla.Topologies
{
    public interface IMessageTopology
    {
        IQueue SubscribeQueue { get; }

        IBindable PublishTarget { get; }

        void Visit(ITopologyVisitor visitor);
    }
}
