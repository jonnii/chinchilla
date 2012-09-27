using System.Threading;
using Chinchilla.Integration.Features.Messages;
using NUnit.Framework;

namespace Chinchilla.Integration.Features
{
    [TestFixture]
    public class SharedSubscriptionFeature : WithApi
    {
        [Test]
        public void ShouldReceivedPublishedMessage()
        {
            using (var bus = Depot.Connect("localhost/integration"))
            {
                var numReceived = 0;

                bus.Subscribe((HelloWorldMessage hwm) =>
                {
                    ++numReceived;
                }, o => o.SubscribeOn("queue-1", "queue-2"));

                var first = bus.CreatePublisher<HelloWorldMessage>(o => o.PublishOn("hello-exchange-1"));
                var second = bus.CreatePublisher<HelloWorldMessage>(o => o.PublishOn("hello-exchange-2"));

                for (var i = 0; i < 50; ++i)
                {
                    first.Publish(new HelloWorldMessage { Message = "subscribe!" });
                    second.Publish(new HelloWorldMessage { Message = "subscribe!" });
                }

                Thread.Sleep(1000);

                Assert.That(numReceived, Is.EqualTo(100));
            }
        }
    }
}
