using System.Linq;
using System.Threading.Tasks;
using Chinchilla.Api;
using Chinchilla.Integration.Features.Messages;
using Xunit;

namespace Chinchilla.Integration.Features
{
    [Collection("Rabbit Collection")]
    public class PublishFeature
    {
        private readonly IRabbitAdmin admin;

        private readonly VirtualHost vhost;

        public PublishFeature(RabbitFixture fixture)
        {
            admin = fixture.Admin;
            vhost = fixture.IntegrationVHost;
        }

        [Fact]
        public async Task ShouldPublishMessageOnDefaultPublisher()
        {
            using (var bus = Depot.Connect("localhost/integration"))
            {
                bus.Publish(new HelloWorldMessage());
            }

            var exchanges = await admin.ExchangesAsync(vhost);

            Assert.Contains("HelloWorldMessage", exchanges.Select(e => e.Name));
        }

        [Fact]
        public async Task ShouldCreatePublisher()
        {
            using (var bus = Depot.Connect("localhost/integration"))
            {
                using (var publisher = bus.CreatePublisher<HelloWorldMessage>())
                {
                    publisher.Publish(new HelloWorldMessage());
                    Assert.Equal(1, publisher.NumPublishedMessages);
                }
            }

            var exchanges = await admin.ExchangesAsync(vhost);

            Assert.Contains("HelloWorldMessage", exchanges.Select(e => e.Name));
        }

        [Fact]
        public async Task ShouldCreatePublisherWithCustomPublishExchange()
        {
            using (var bus = Depot.Connect("localhost/integration"))
            {
                using (var publisher = bus.CreatePublisher<HelloWorldMessage>(o => o.PublishOn("custom-exchange-name")))
                {
                    publisher.Publish(new HelloWorldMessage());
                    Assert.Equal(1, publisher.NumPublishedMessages);
                }
            }

            var exchanges = await admin.ExchangesAsync(vhost);

            Assert.Contains("custom-exchange-name", exchanges.Select(e => e.Name));
        }

        [Fact]
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

                    Assert.Equal(100, publisher.NumPublishedMessages);
                }
            }
        }

        [Fact]
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

                    Assert.Equal(100, publisher.NumPublishedMessages);
                }
            }
        }

        // [Test]
        // public void ShouldPublishFromMultipleThreads()
        // {
        //     using (var bus = Depot.Connect("localhost/integration"))
        //     {
        //         using (var publisher = bus.CreatePublisher<HelloWorldMessage>())
        //         {
        //             var threads = Enumerable.Range(0, 10).Select(_ => new Thread(() =>
        //             {
        //                 for (var i = 0; i < 100; ++i)
        //                 {
        //                     publisher.Publish(new HelloWorldMessage());
        //                 }
        //             })).ToArray();

        //             foreach (var thread in threads)
        //             {
        //                 thread.Start();
        //             }

        //             foreach (var thread in threads)
        //             {
        //                 thread.Join();
        //             }

        //             WaitForDelivery();

        //             Assert.That(publisher.NumPublishedMessages, Is.EqualTo(1000));
        //         }
        //     }
        // }

        // [Test]
        // public void ShouldHaveCustomPublisherFaultStrategy()
        // {
        //     using (var bus = Depot.Connect("localhost/integration"))
        //     {
        //         using (var publisher = bus.CreatePublisher<HelloWorldMessage>(
        //             o => o.Confirm(true).OnFailure<RetryOnFailures>()))
        //         {
        //             // TODO: Find a way to make this nack, so we can test the fault strategy
        //             publisher.Publish(new HelloWorldMessage());

        //             WaitForDelivery();
        //         }
        //     }
        // }

        public class CustomRouter : DefaultRouter
        {
            public override string Route<TMessage>(TMessage message)
            {
                return "#";
            }
        }

        public class RetryOnFailures : IPublisherFailureStrategy<HelloWorldMessage>
        {
            public void OnFailure(IPublisher<HelloWorldMessage> publisher, HelloWorldMessage failedMessage, IPublishReceipt receipt)
            {
                publisher.Publish(failedMessage);
            }
        }
    }
}
