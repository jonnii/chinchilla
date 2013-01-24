using System.Linq;
using System.Threading;
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
        public void ShouldCreatePublisherWithCustomPublishExchange()
        {
            using (var bus = Depot.Connect("localhost/integration"))
            {
                using (var publisher = bus.CreatePublisher<HelloWorldMessage>(o => o.PublishOn("custom-exchange-name")))
                {
                    publisher.Publish(new HelloWorldMessage());
                    Assert.That(publisher.NumPublishedMessages, Is.EqualTo(1));
                }
            }

            Assert.That(admin.Exchanges(IntegrationVHost).Any(e => e.Name == "custom-exchange-name"));
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

        [Test]
        public void ShouldPublishFromMultipleThreads()
        {
            using (var bus = Depot.Connect("localhost/integration"))
            {
                using (var publisher = bus.CreatePublisher<HelloWorldMessage>())
                {
                    var threads = Enumerable.Range(0, 10).Select(_ => new Thread(() =>
                    {
                        for (var i = 0; i < 100; ++i)
                        {
                            publisher.Publish(new HelloWorldMessage());
                        }
                    })).ToArray();

                    foreach (var thread in threads)
                    {
                        thread.Start();
                    }

                    foreach (var thread in threads)
                    {
                        thread.Join();
                    }

                    Thread.Sleep(5000);

                    Assert.That(publisher.NumPublishedMessages, Is.EqualTo(1000));
                }
            }
        }

        public class CustomRouter : DefaultRouter
        {
            public override string Route<TMessage>(TMessage message)
            {
                return "#";
            }
        }
    }
}
