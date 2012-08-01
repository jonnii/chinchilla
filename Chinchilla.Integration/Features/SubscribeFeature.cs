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
                HelloWorldMessage received = null;
                using (bus.Subscribe((HelloWorldMessage hwm) => { received = hwm; }))
                {
                    bus.Publish(new HelloWorldMessage { Message = "subscribe!" });

                    Thread.Sleep(1000);
                }

                Assert.That(received, Is.Not.Null);
                Assert.That(received.Message, Is.EqualTo("subscribe!"));
            }
        }
    }
}
