using System.Collections.Generic;

namespace Chinchilla.Topologies.Rabbit
{
    public class Topology : ITopology
    {
        private readonly List<Queue> queues = new List<Queue>();

        private readonly List<Exchange> exchanges = new List<Exchange>();

        public IQueue DefineQueue()
        {
            var queue = new Queue();
            queues.Add(queue);
            return queue;
        }

        public IQueue DefineQueue(string name)
        {
            var queue = new Queue(name);
            queues.Add(queue);
            return queue;
        }

        public IExchange DefineExchange(string name, ExchangeType exchangeType)
        {
            var exchange = new Exchange(name, exchangeType);
            exchanges.Add(exchange);
            return exchange;
        }

        public void Visit(ITopologyVisitor visitor)
        {
            foreach (var exchange in exchanges)
            {
                exchange.Visit(visitor);
            }

            foreach (var queue in queues)
            {
                queue.Visit(visitor);
            }
        }
    }
}