using System.Linq;
using System.Threading.Tasks;
using Chinchilla.Integration.Features.Messages;
using Xunit;

namespace Chinchilla.Integration.Features
{
    public class PublisherConfirmsFeature : Feature
    {
        [Fact]
        public async Task ShouldCreatePublisherWithoutConfirms()
        {
            using (var bus = await CreateBus())
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
        public async Task ShouldWaitForAllMessagesToBeConfirmedWhenDisposing()
        {
            using (var bus = await CreateBus())
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
