using Chinchilla.Topologies.Model;

namespace Chinchilla.Topologies
{
    public interface IMessageTopology
    {
        IQueue SubscribeQueue { get; }

        IExchange PublishExchange { get; }

        void Visit(ITopologyVisitor visitor);
    }
}
