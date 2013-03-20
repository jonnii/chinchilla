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

            bus.Subscribe<TimeoutMessage>(
                OnMessageTimeout,
                s => s.SetTopology<TimeoutSubscriptionTopology>());

            using (var requester = bus.CreateRequester<TimeoutMessage, TimeoutResponse>())
            {
                while (isRunning)
                {
                    Console.WriteLine("[Publisher] Publishing new timeout message");

                    requester.Request(new TimeoutMessage { JobId = Guid.NewGuid().ToString() }, OnResponse);
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

        public void Stop()
        {
            isRunning = false;
            bus.Dispose();
        }
    }
}
