using System.Threading.Tasks;
using Chinchilla.Api;
using Chinchilla.Integration.Features.Consumers;
using Chinchilla.Integration.Features.Messages;
using Xunit;

namespace Chinchilla.Integration.Features
{
    public class ConsumerFeature : Feature
    {
        [Fact]
        public async Task ShouldSubscribeWithConsumerInstance()
        {
            using (var bus = await CreateBus())
            {
                using (var subscriber = bus.Subscribe(new HelloWorldMessageConsumer()))
                {
                    for (var i = 0; i < 100; ++i)
                    {
                        bus.Publish(new HelloWorldMessage { Message = "subscribe!" });
                    }

                    var s = subscriber;
                    await WaitFor(() => s.State.TotalAcceptedMessages() == 100);

                    Assert.Equal(100, subscriber.State.TotalAcceptedMessages());
                }
            }
        }

        [Fact]
        public async Task ShouldSubscribeWithConsumerType()
        {
            using (var bus = await CreateBus())
            {
                using (var subscriber = bus.Subscribe<HelloWorldMessageConsumer>())
                {
                    for (var i = 0; i < 100; ++i)
                    {
                        bus.Publish(new HelloWorldMessage { Message = "subscribe!" });
                    }

                    var s = subscriber;
                    await WaitFor(() => s.State.TotalAcceptedMessages() > 100);

                    Assert.Equal(100, subscriber.State.TotalAcceptedMessages());
                }
            }
        }

        [Fact]
        public async Task ShouldSubscribeWithConsumerWithCustomConfiguration()
        {
            using (var bus = await CreateBus())
            {
                using (bus.Subscribe<CustomConfigurationConsumer>())
                {
                    for (var i = 0; i < 1; ++i)
                    {
                        bus.Publish(new HelloWorldMessage { Message = "subscribe!" });
                    }

                    //await Task.Delay(1000);

                    var tt = await Admin.ExistsAsync(VirtualHost, new Queue("custom-subscription-endpoint"));

                    Assert.True(tt);
                }
            }
        }
    }
}
