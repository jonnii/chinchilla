using System;
using System.Threading;

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
            var builder = new SubscriberTopology("messages.slow");

            bus.Subscribe<SharedMessage>(
                ProcessMessage,
                a => a.SetTopology(builder)
                    .SubscribeOn("slow-messages", "fast-messages")
                    .WithPrefetchCount(2)
                    .DeliverUsing<WorkerPoolDeliveryStrategy>(s => s.NumWorkers = 2));
        }

        public void ProcessMessage(SharedMessage message)
        {
            Console.WriteLine("Processing (slow) {0}", message);

            var messageProcessingTime = message.MessageType == MessageType.Slow
                ? 50000
                : 1000;

            if (message.MessageType == MessageType.Slow)
            {
                Console.WriteLine("starting slow message " + message.Id);
            }

            Thread.Sleep(messageProcessingTime);

            if (message.MessageType == MessageType.Slow)
            {
                Console.WriteLine("finishing slow message " + message.Id);
            }
        }

        public void Stop()
        {
            bus.Dispose();
        }
    }
}