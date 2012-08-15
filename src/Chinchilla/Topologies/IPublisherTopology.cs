using Chinchilla.Topologies.Rabbit;

namespace Chinchilla.Topologies
{
    public interface IPublisherTopology
    {
        /// <summary>
        /// The queue that will receive message, this will be subscribed to
        /// </summary>
        IExchange Exchange { get; }

        /// <summary>
        /// Visits this subscription topology with a topology visitor
        /// </summary>
        /// <param name="visitor"></param>
        void Visit(ITopologyVisitor visitor);
    }
}