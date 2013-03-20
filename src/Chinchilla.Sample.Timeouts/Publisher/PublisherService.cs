using System;
using System.Threading;
using Chinchilla.Sample.Timeouts.Messages;

namespace Chinchilla.Sample.Timeouts.Publisher
{
    public class PublisherService
    {
        private readonly IBus bus;

        private bool isRunning;

        public PublisherService()
        {
            bus = Depot.Connect("localhost/timeout");
        }

        public void Start()
        {
            isRunning = true;

            // create a subscription to timeout messages 
            // with a custom timeout subscription topology
            var subscription = bus.Subscribe<TimeoutMessage>(
                OnMessageTimeout,
                s => s.SetTopology<TimeoutSubscriptionTopology>());

            // get the name of our subscription queue
            var timeoutSubscriptionQueueName = subscription.Queues[0].Name;

            // this subscription should never be fired
            // create a subscription to timeout messages 
            // with a custom timeout subscription topology
            bus.Subscribe<TimeoutMessage>(
                OnMessageTimeoutAlt,
                s => s.SetTopology<TimeoutSubscriptionTopology>());

            // create a standard requester
            using (var requester = bus.CreateRequester<TimeoutMessage, TimeoutResponse>())
            {
                while (isRunning)
                {
                    Console.WriteLine("[Publisher] Publishing new timeout message");

                    // set the routing key on each message to the subscription
                    // queue name, this will route our timeout message to our
                    // timeout subscription
                    var message = new TimeoutMessage
                    {
                        JobId = Guid.NewGuid().ToString(),
                        RoutingKey = timeoutSubscriptionQueueName
                    };

                    requester.Request(message, OnResponse);
                    Thread.Sleep(3000);
                }
            }
        }

        private void OnResponse(TimeoutResponse message)
        {
            Console.WriteLine("[Publisher] Got Response");
        }

        private void OnMessageTimeout(TimeoutMessage message)
        {
            Console.WriteLine("[Publisher] MESSAGE TIMEDOUT: {0}", message.JobId);
        }

        private void OnMessageTimeoutAlt(TimeoutMessage message)
        {
            throw new Exception("Other timeout subscriber, should never be called");
        }

        public void Stop()
        {
            isRunning = false;
            bus.Dispose();
        }
    }
}
