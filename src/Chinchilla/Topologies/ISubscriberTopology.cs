using Chinchilla.Topologies.Model;

namespace Chinchilla.Topologies
{
    /// <summary>
    /// A subscription topology is the strategy by which a subscriber will receive messages.
    /// </summary>
    public interface ISubscriberTopology
    {
        /// <summary>
        /// The queue that will receive message, this will be subscribed to
        /// </summary>
        IQueue SubscribeQueue { get; }

        void Visit(ITopologyVisitor visitor);
    }
}
