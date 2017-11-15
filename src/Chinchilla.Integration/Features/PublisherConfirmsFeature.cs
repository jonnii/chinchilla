using System.Linq;
using Chinchilla.Api;
using Chinchilla.Integration.Features.Messages;
using Xunit;

namespace Chinchilla.Integration.Features
{
    [Collection("Rabbit Collection")]
    public class PublisherConfirmsFeature
    {
        private readonly IRabbitAdmin admin;

        private readonly VirtualHost vhost;

        public PublisherConfirmsFeature(RabbitFixture fixture)
        {
            admin = fixture.Admin;
            vhost = fixture.IntegrationVHost;
        }

        [Fact]
        public void ShouldCreatePublisherWithoutConfirms()
        {
            using (var bus = Depot.Connect("localhost/integration"))
            {
                var publisher = bus.CreatePublisher<HelloWorldMessage>(p => p.Confirm(false));

                var receipts = Enumerable.Range(0, 100)
                    .Select(_ => publisher.Publish(new HelloWorldMessage()))
                    .ToArray();

                publisher.Dispose();

                Assert.True(receipts.All(r => r.Status == PublishStatus.None));
            }
        }

        [Fact]
        public void ShouldWaitForAllMessagesToBeConfirmedWhenDisposing()
        {
            using (var bus = Depot.Connect("localhost/integration"))
            {
                var publisher = bus.CreatePublisher<HelloWorldMessage>(p => p.Confirm(true));

                var receipts = Enumerable.Range(0, 100)
                    .Select(_ => publisher.Publish(new HelloWorldMessage()))
                    .ToArray();

                publisher.Dispose();

                Assert.True(receipts.All(r => r.IsConfirmed));
            }
        }
    }
}
