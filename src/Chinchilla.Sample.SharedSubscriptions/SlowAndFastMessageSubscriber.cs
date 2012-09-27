using System;
using System.Threading;
using Chinchilla.Topologies;

namespace Chinchilla.Sample.SharedSubscriptions
{
    public class SlowAndFastMessageSubscriber
    {
        private readonly IBus bus;

        private ISubscription subscription;

        public SlowAndFastMessageSubscriber()
        {
            bus = Depot.Connect("localhost/shared-subscriptions");
        }

        public void Start()
        {
            var builder = new DefaultSubscribeTopologyBuilder();

            subscription = bus.Subscribe<SharedMessage>(
                ProcessMessage,
                a => a.SetTopology(builder)
                    .SubscribeOn("slow-messages", "fast-messages")
                    .DeliverUsing<WorkerPoolDeliveryStrategy>(s => s.NumWorkers = 1));
        }

        public void ProcessMessage(SharedMessage message)
        {
            Console.WriteLine("Processing (slow) {0}", message);

            var messageProcessingTime = message.MessageType == MessageType.Slow
                ? 10000
                : 3000;

            Thread.Sleep(messageProcessingTime);
        }

        public void Stop()
        {
            subscription.Dispose();
            bus.Dispose();
        }
    }
}