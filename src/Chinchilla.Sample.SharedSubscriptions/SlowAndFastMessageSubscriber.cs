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
            var builder = new DefaultSubscribeTopologyBuilder("messages.slow");

            subscription = bus.Subscribe<SharedMessage>(
                ProcessMessage, a => a.SetTopology(builder).SubscribeOn("slow-messages", "fast-messages"));
        }

        public void ProcessMessage(SharedMessage message)
        {
            Console.WriteLine("Processing (slow) {0}", message);
            Thread.Sleep(5000);
        }

        public void Stop()
        {
            subscription.Dispose();
            bus.Dispose();
        }
    }
}