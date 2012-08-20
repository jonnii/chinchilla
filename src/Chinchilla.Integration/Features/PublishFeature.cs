using System.Linq;
using Chinchilla.Integration.Features.Messages;
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
                using (var publisher = bus.CreatePublisher<HelloWorldMessage>())
                {
                    publisher.Publish(new HelloWorldMessage());
                    Assert.That(publisher.NumPublishedMessages, Is.EqualTo(1));
                }
            }

            Assert.That(admin.Exchanges(IntegrationVHost).Any(e => e.Name == "HelloWorldMessage"));
        }

        [Test]
        public void ShouldPublishMultipleMessages()
        {
            using (var bus = Depot.Connect("localhost/integration"))
            {
                using (var publisher = bus.CreatePublisher<HelloWorldMessage>())
                {
                    for (var i = 0; i < 100; ++i)
                    {
                        publisher.Publish(new HelloWorldMessage());
                    }

                    Assert.That(publisher.NumPublishedMessages, Is.EqualTo(100));
                }
            }
        }

        [Test]
        public void ShouldPublishWithCustomRouter()
        {
            using (var bus = Depot.Connect("localhost/integration"))
            {
                var publisher = bus.CreatePublisher<HelloWorldMessage>(o => o.RouteWith<CustomRouter>());

                using (publisher)
                {
                    for (var i = 0; i < 100; ++i)
                    {
                        publisher.Publish(new HelloWorldMessage());
                    }

                    Assert.That(publisher.NumPublishedMessages, Is.EqualTo(100));
                }
            }
        }

        public class CustomRouter : IRouter
        {
            public string Route<TMessage>(TMessage message)
            {
                return "#";
            }
        }
    }
}
