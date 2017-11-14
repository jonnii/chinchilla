using System.Threading.Tasks;
using Chinchilla.Api;
using Chinchilla.Integration.Features.Consumers;
using Chinchilla.Integration.Features.Messages;
using Xunit;

namespace Chinchilla.Integration.Features
{
    [Collection("Api collection")]
    public class ConsumerFeature : Feature
    {
        private readonly IRabbitAdmin admin;

        private readonly VirtualHost vhost;

        public ConsumerFeature(ApiFixture fixture)
        {
            admin = fixture.Admin;
            vhost = fixture.IntegrationVHost;
        }

        // [Fact]
        // public void ShouldSubscribeWithConsumerInstance()
        // {
        //     using (var bus = Depot.Connect("localhost/integration"))
        //     {
        //         using (var subscriber = bus.Subscribe(new HelloWorldMessageConsumer()))
        //         {
        //             for (var i = 0; i < 100; ++i)
        //             {
        //                 bus.Publish(new HelloWorldMessage { Message = "subscribe!" });
        //             }

        //             WaitFor(() => subscriber.State.TotalAcceptedMessages() == 100);

        //             Assert.Equal(100, subscriber.State.TotalAcceptedMessages());
        //         }
        //     }
        // }

        // [Fact]
        // public void ShouldSubscribeWithConsumerType()
        // {
        //     using (var bus = Depot.Connect("localhost/integration"))
        //     {
        //         using (var subscriber = bus.Subscribe<HelloWorldMessageConsumer>())
        //         {
        //             for (var i = 0; i < 100; ++i)
        //             {
        //                 bus.Publish(new HelloWorldMessage { Message = "subscribe!" });
        //             }

        //             WaitFor(() => subscriber.State.TotalAcceptedMessages() > 100);

        //             Assert.Equal(100, subscriber.State.TotalAcceptedMessages());
        //         }
        //     }
        // }

        [Fact]
        public async Task ShouldSubscribeWithConsumerWithCustomConfiguration()
        {
            using (var bus = Depot.Connect("localhost/integration"))
            {
                using (bus.Subscribe<CustomConfigurationConsumer>())
                {
                    for (var i = 0; i < 1; ++i)
                    {
                        bus.Publish(new HelloWorldMessage { Message = "subscribe!" });
                    }

                    // WaitFor(() => true);

                    Assert.True(await admin.ExistsAsync(vhost, new Queue("custom-subscription-endpoint")));
                }
            }
        }
    }
}
