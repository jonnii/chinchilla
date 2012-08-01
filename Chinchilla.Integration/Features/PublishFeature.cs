using System.Linq;
using NUnit.Framework;

namespace Chinchilla.Integration.Features
{
    [TestFixture]
    public class PublishFeature : WithApi
    {
        [Test]
        public void ShouldPublishMessageOnDefaultPublisher()
        {
            using (var bus = Depot.Connect("localhost/integration"))
            {
                bus.Publish(new HelloWorldMessage());
            }

            Assert.That(admin.Exchanges(IntegrationVHost).Any(e => e.Name == "HelloWorldMessage"));
        }

        [Test]
        public void ShouldCreatePublisher()
        {
            using (var bus = Depot.Connect("localhost/integration"))
            {
                using (var publisher = bus.CreatePublishChannel())
                {
                    publisher.Publish(new HelloWorldMessage());
                    Assert.That(publisher.PublishedMessages, Is.EqualTo(1));
                }
            }

            Assert.That(admin.Exchanges(IntegrationVHost).Any(e => e.Name == "HelloWorldMessage"));
        }

        [Test]
        public void ShouldPublishMultipleMessages()
        {
            using (var bus = Depot.Connect("localhost/integration"))
            {
                using (var publisher = bus.CreatePublishChannel())
                {
                    for (var i = 0; i < 100; ++i)
                    {
                        publisher.Publish(new HelloWorldMessage());
                    }

                    Assert.That(publisher.PublishedMessages, Is.EqualTo(100));
                }
            }
        }
    }
}
