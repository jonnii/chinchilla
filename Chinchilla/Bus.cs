using System;
using Chinchilla.Topologies.Rabbit;

namespace Chinchilla
{
    public class Bus : IBus
    {
        public Bus()
        {
            Topology = new Topology();
        }

        public ITopology Topology { get; private set; }

        public ISubscription Subscribe<T>(Action<T> onMessage)
        {
            var queue = Topology.DefineQueue();
            var exchange = Topology.DefineExchange(typeof(T).Name, ExchangeType.Fanout);

            queue.BindTo(exchange);

            return Subscribe(queue, exchange, onMessage);
        }

        private ISubscription Subscribe<T>(IQueue queue, IExchange exchange, Action<T> onMessage)
        {
            return new SubscriptionHandle(queue);
        }

        public void Publish<T>(T message)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }
    }
}