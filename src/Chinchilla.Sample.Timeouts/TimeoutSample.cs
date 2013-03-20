using System;
using System.Threading;
using Chinchilla.Sample.Timeouts.Consumer;
using Chinchilla.Sample.Timeouts.Publisher;

namespace Chinchilla.Sample.Timeouts
{
    public class TimeoutSample : IDisposable
    {
        private readonly PublisherService publisher = new PublisherService();

        private readonly ConsumerService consumer = new ConsumerService();

        public void Run()
        {
            var publisherThread = new Thread(() => publisher.Start());
            publisherThread.Start();

            var consumerThread = new Thread(() => consumer.Start());
            consumerThread.Start();
        }

        public void Dispose()
        {
            publisher.Stop();
            consumer.Stop();
        }
    }
}