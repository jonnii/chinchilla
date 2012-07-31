using System.Collections.Generic;

namespace Chinchilla.Topologies.Rabbit
{
    public class Topology : ITopology
    {
        private readonly List<Queue> queues = new List<Queue>();

        public IQueue DefineQueue()
        {
            var queue = new Queue();
            queues.Add(queue);
            return queue;
        }

        public IExchange DefineExchange(string name, ExchangeType exchangeType)
        {
            return new Exchange(name, exchangeType);
        }

        public void Visit(ITopologyVisitor visitor)
        {
            foreach (var queue in queues)
            {
                queue.Visit(visitor);
            }
        }
    }
}