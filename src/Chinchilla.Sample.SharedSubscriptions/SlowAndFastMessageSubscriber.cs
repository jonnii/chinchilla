using System;
using System.Threading;
using Chinchilla.Topologies;

namespace Chinchilla.Sample.SharedSubscriptions
{
    public class SlowAndFastMessageSubscriber
    {
        private readonly IBus bus;

        public SlowAndFastMessageSubscriber()
        {
            bus = Depot.Connect("localhost/shared-subscriptions");
        }

        public void Start()
        {
            var builder = new DefaultSubscribeTopologyBuilder();

            bus.Subscribe<SharedMessage>(
                ProcessMessage,
                a => a.SetTopology(builder)
                    .SubscribeOn("slow-messages", "fast-messages")
                    .WithPrefetchCount(1)
                    .DeliverUsing<WorkerPoolDeliveryStrategy>(s => s.NumWorkers = 1));
        }

        public void ProcessMessage(SharedMessage message)
        {
            Console.WriteLine("Processing (slow) {0}", message);

            var messageProcessingTime = message.MessageType == MessageType.Slow
                ? 20000
                : 3000;

            if (message.MessageType == MessageType.Slow)
            {
                Console.WriteLine("starting slow message");
            }

            Thread.Sleep(messageProcessingTime);

            if (message.MessageType == MessageType.Slow)
            {
                Console.WriteLine("finishing slow message");
            }
        }

        public void Stop()
        {
            bus.Dispose();
        }
    }
}