using System.Collections.Generic;

namespace Chinchilla.Topologies.Model
{
    public class Topology : ITopology
    {
        private readonly List<IQueue> queues = new List<IQueue>();

        private readonly List<IExchange> exchanges = new List<IExchange>();

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

        public IExchange DefineExchange(
            string name,
            ExchangeType exchangeType,
            Durability durablility = Durability.Durable,
            bool isAutoDelete = false)
        {
            var exchange = new Exchange(name, exchangeType)
            {
                IsAutoDelete = isAutoDelete,
                Durability = durablility
            };

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