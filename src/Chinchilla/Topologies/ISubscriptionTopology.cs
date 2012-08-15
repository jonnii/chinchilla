using Chinchilla.Topologies.Rabbit;

namespace Chinchilla.Topologies
{
    /// <summary>
    /// A subscription topology is the strategy by which a subscriber will receive messages.
    /// </summary>
    public interface ISubscriptionTopology
    {
        /// <summary>
        /// The queue that will receive message, this will be subscribed to
        /// </summary>
        IQueue Queue { get; }

        /// <summary>
        /// Visits this subscription topology with a topology visitor
        /// </summary>
        /// <param name="visitor"></param>
        void Visit(ITopologyVisitor visitor);
    }
}
