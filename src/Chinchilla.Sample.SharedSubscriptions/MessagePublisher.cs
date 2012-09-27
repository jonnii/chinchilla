using System.Threading;

namespace Chinchilla.Sample.SharedSubscriptions
{
    public class MessagePublisher
    {
        private readonly IBus bus;

        private bool isRunning;

        public MessagePublisher()
        {
            bus = Depot.Connect("localhost/shared-subscriptions");
        }

        public void Start()
        {
            isRunning = true;

            var messageIndex = 0;

            using (var publisher = bus.CreatePublisher<SharedMessage>(
                o => o.RouteWith<SharedMessageRouter>().SetTopology(new MessagePublisherTopology())))
            {
                while (isRunning)
                {
                    var message = messageIndex % 20 == 0
                        ? new SharedMessage(messageIndex, MessageType.Slow)
                        : new SharedMessage(messageIndex, MessageType.Fast);

                    publisher.Publish(message);

                    Thread.Sleep(2000);

                    ++messageIndex;
                }
            }
        }

        public void Stop()
        {
            bus.Dispose();
            isRunning = false;
        }
    }
}