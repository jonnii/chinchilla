using System;
using System.Threading;
using Chinchilla.Api;
using Chinchilla.Integration.Features.Messages;
using NUnit.Framework;

namespace Chinchilla.Integration.Features
{
    [TestFixture]
    public class SubscribeFeature : WithApi
    {
        [Test]
        public void ShouldSubscribeMessage()
        {
            using (var bus = Depot.Connect("localhost/integration"))
            {
                using (bus.Subscribe((HelloWorldMessage hwm) => { }))
                {
                    Assert.That(admin.Exists(IntegrationVHost, new Queue("HelloWorldMessage")), "did not create queue");
                    Assert.That(admin.Exists(IntegrationVHost, new Exchange("HelloWorldMessage")), "did not create exchange");
                }
            }
        }

        [Test]
        public void ShouldReceivedPublishedMessage()
        {
            using (var bus = Depot.Connect("localhost/integration"))
            {
                HelloWorldMessage lastReceived = null;
                var numReceived = 0;
                using (bus.Subscribe((HelloWorldMessage hwm) => { lastReceived = hwm; ++numReceived; }))
                {
                    for (var i = 0; i < 100; ++i)
                    {
                        bus.Publish(new HelloWorldMessage { Message = "subscribe!" });
                    }

                    Thread.Sleep(1000);
                }

                Assert.That(lastReceived, Is.Not.Null);
                Assert.That(lastReceived.Message, Is.EqualTo("subscribe!"));
                Assert.That(numReceived, Is.EqualTo(100));
            }
        }

        [Test]
        public void ShouldCreateSubscriberWithMaxConsumers()
        {
            using (var bus = Depot.Connect("localhost/integration"))
            {
                var handler = new Action<HelloWorldMessage>(hwm => { });
                using (bus.Subscribe(handler, o => o.MaxConsumers(2)))
                {

                }
            }
        }
    }
}
