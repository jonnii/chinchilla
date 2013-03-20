using System;
using System.Threading;
using Chinchilla.Configuration;
using Chinchilla.Sample.Timeouts.Messages;

namespace Chinchilla.Sample.Timeouts.Consumer
{
    public class ConsumerService
    {
        private readonly IBus bus;

        public ConsumerService()
        {
            bus = Depot.Connect("localhost/timeout");
        }

        public void Start()
        {
            bus.Subscribe<TimeoutMessage>(OnMessage, ConfigureWithDeadLetter);
        }

        private void ConfigureWithDeadLetter(ISubscriptionBuilder builder)
        {
            builder
                .SetTopology<DeadLetterTopology>()
                .DeliverUsing<WorkerPoolDeliveryStrategy>(s => s.NumWorkers = 1)
                .WithPrefetchCount(1);
        }

        private void OnMessage(TimeoutMessage message, IDeliveryContext context)
        {
            Console.WriteLine("[Consumer] Received message: {0}", message.JobId);
            Thread.Sleep(8000);
            Console.WriteLine("[Consumer] Processed message: {0}", message.JobId);

            context.Reply(new TimeoutResponse());
        }

        public void Stop()
        {
            bus.Dispose();
        }
    }
}
