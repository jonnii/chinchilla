using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Chinchilla.Integration.Features.Messages;
using Xunit;

namespace Chinchilla.Integration.Features
{
    public class PublishFeature : Feature
    {
        [Fact]
        public async Task ShouldPublishMessageOnDefaultPublisher()
        {
            using (var bus = await CreateBus())
            {
                bus.Publish(new HelloWorldMessage());
            }

            var exchanges = await Admin.ExchangesAsync(VirtualHost);

            Assert.Contains("HelloWorldMessage", exchanges.Select(e => e.Name));
        }

        [Fact]
        public async Task ShouldCreatePublisher()
        {
            using (var bus = await CreateBus())
            {
                using (var publisher = bus.CreatePublisher<HelloWorldMessage>())
                {
                    publisher.Publish(new HelloWorldMessage());
                    Assert.Equal(1, publisher.NumPublishedMessages);
                }
            }

            var exchanges = await Admin.ExchangesAsync(VirtualHost);

            Assert.Contains("HelloWorldMessage", exchanges.Select(e => e.Name));
        }

        [Fact]
        public async Task ShouldCreatePublisherWithCustomPublishExchange()
        {
            using (var bus = await CreateBus())
            {
                using (var publisher = bus.CreatePublisher<HelloWorldMessage>(o => o.PublishOn("custom-exchange-name")))
                {
                    publisher.Publish(new HelloWorldMessage());
                    Assert.Equal(1, publisher.NumPublishedMessages);
                }
            }

            var exchanges = await Admin.ExchangesAsync(VirtualHost);

            Assert.Contains("custom-exchange-name", exchanges.Select(e => e.Name));
        }

        [Fact]
        public async Task ShouldPublishMultipleMessages()
        {
            using (var bus = await CreateBus())
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
        public async Task ShouldPublishWithCustomRouter()
        {
            using (var bus = await CreateBus())
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

        [Fact]
        public async Task ShouldPublishFromMultipleThreads()
        {
            using (var bus = await CreateBus())
            {
                using (var publisher = bus.CreatePublisher<HelloWorldMessage>())
                {
                    var p = publisher;

                    var threads = Enumerable.Range(0, 10).Select(_ => new Thread(() =>
                    {
                        for (var i = 0; i < 100; ++i)
                        {
                            p.Publish(new HelloWorldMessage());
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

                    await WaitFor(() => p.NumPublishedMessages == 1000);

                    Assert.Equal(1000, publisher.NumPublishedMessages);
                }
            }
        }

        //[Fact]
        //public async Task ShouldHaveCustomPublisherFaultStrategy()
        //{
        //    using (var bus = await CreateBus())
        //    {
        //        using (var publisher = bus.CreatePublisher<HelloWorldMessage>(
        //            o => o.Confirm(true).OnFailure<RetryOnFailures>()))
        //        {
        //            // TODO: Find a way to make this nack, so we can test the fault strategy
        //            publisher.Publish(new HelloWorldMessage());

        //            WaitForDelivery();
        //        }
        //    }
        //}

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
