using System;
using System.Threading;

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
            var builder = new SubscriberTopology("messages.fast");

            bus.Subscribe<SharedMessage>(
                ProcessMessage,
                a => a.SetTopology(builder)
                    .SubscribeOn("fast-messages")
                    .WithPrefetchCount(1)
                    .DeliverUsing<WorkerPoolDeliveryStrategy>(s => s.NumWorkers = 1));
        }

        public void ProcessMessage(SharedMessage message)
        {
            if (message.MessageType == MessageType.Slow)
            {
                throw new Exception("Not supported");
            }

            Console.WriteLine("Processing (fast) {0}", message);
            Thread.Sleep(1000);
        }

        public void Stop()
        {
            bus.Dispose();
        }
    }
}