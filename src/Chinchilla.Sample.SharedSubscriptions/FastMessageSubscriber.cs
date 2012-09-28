using System;
using System.Threading;
using Chinchilla.Topologies;

namespace Chinchilla.Sample.SharedSubscriptions
{
    public class FastMessageSubscriber
    {
        private readonly IBus bus;

        public FastMessageSubscriber()
        {
            bus = Depot.Connect("localhost/shared-subscriptions");
        }

        public void Start()
        {
            var builder = new DefaultSubscribeTopologyBuilder();

            bus.Subscribe<SharedMessage>(
                ProcessMessage,
                a => a.SetTopology(builder)
                    .SubscribeOn("fast-messages")
                    .WithPrefetchCount(1)
                    .DeliverUsing<WorkerPoolDeliveryStrategy>(s => s.NumWorkers = 1));
        }

        public void ProcessMessage(SharedMessage message)
        {
            Console.WriteLine("Processing (fast) {0}", message);
            Thread.Sleep(3000);
        }

        public void Stop()
        {
            bus.Dispose();
        }
    }
}