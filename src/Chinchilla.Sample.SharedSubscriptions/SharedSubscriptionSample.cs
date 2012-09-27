using System;
using System.Threading;

namespace Chinchilla.Sample.SharedSubscriptions
{
    public class SharedSubscriptionSample : IDisposable
    {
        private readonly MessagePublisher server = new MessagePublisher();

        private readonly FastMessageSubscriber fastClient = new FastMessageSubscriber();

        private readonly SlowMessageSubscriber slowClient = new SlowMessageSubscriber();

        public void Run()
        {
            var serverThread = new Thread(() => server.Start());
            serverThread.Start();

            var fastThread = new Thread(() => fastClient.Start());
            fastThread.Start();

            var slowThread = new Thread(() => slowClient.Start());
            slowThread.Start();
        }

        public void Dispose()
        {
            fastClient.Stop();
            slowClient.Stop();
            server.Stop();
        }
    }
}