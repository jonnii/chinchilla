using Chinchilla.Topologies.Model;

namespace Chinchilla.Topologies
{
    public interface IPublisherTopology
    {
        /// <summary>
        /// The queue that will receive message, this will be subscribed to
        /// </summary>
        IExchange PublishExchange { get; }

        void Visit(ITopologyVisitor visitor);
    }
}