using System;
using System.Threading;

namespace Chinchilla.Sample.SharedSubscriptions
{
    public class SharedSubscriptionSample : IDisposable
    {
        private readonly MessagePublisher server = new MessagePublisher();

        private readonly FastMessageSubscriber fastClient = new FastMessageSubscriber();

        private readonly SlowAndFastMessageSubscriber slowAndFastClient = new SlowAndFastMessageSubscriber();

        public void Run()
        {
            var serverThread = new Thread(() => server.Start());
            serverThread.Start();

            var fastThread = new Thread(() => fastClient.Start());
            fastThread.Start();

            var slowThread = new Thread(() => slowAndFastClient.Start());
            slowThread.Start();
        }

        public void Dispose()
        {
            fastClient.Stop();
            slowAndFastClient.Stop();
            server.Stop();
        }
    }
}