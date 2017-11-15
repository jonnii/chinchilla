using System.Threading.Tasks;
using Chinchilla.Integration.Features.Messages;
using Chinchilla.Serializers.MsgPack;
using Xunit;

namespace Chinchilla.Integration.Features
{
    public class CustomSerializersFeature : Feature
    {
        [Fact]
        public async Task ShouldHaveDefaultSerializer()
        {
            var settings = new DepotSettings
            {
                MessageSerializers =
                 {
                     Default = new MessagePackMessageSerializer()
                 }
            };

            using (var bus = await CreateBus(settings))
            {
                HelloWorldMessage lastReceived = null;

                bus.Subscribe((HelloWorldMessage hwm) =>
                {
                    lastReceived = hwm;
                });

                bus.Publish(new HelloWorldMessage { Message = "subscribe!" });

                await WaitFor(() => lastReceived != null);

                Assert.NotNull(lastReceived);
                Assert.Equal("subscribe!", lastReceived.Message);
            }
        }

        [Fact]
        public async Task ShouldCustomizePublisherWithContentType()
        {
            var settings = new DepotSettings();
            settings.MessageSerializers.Register(new MessagePackMessageSerializer());

            using (var bus = await CreateBus(settings))
            {
                HelloWorldMessage lastReceived = null;

                bus.Subscribe((HelloWorldMessage hwm) =>
                {
                    lastReceived = hwm;
                });

                using (var publisher = bus.CreatePublisher<HelloWorldMessage>(p => p.SerializeWith("application/x-msgpack")))
                {
                    publisher.Publish(new HelloWorldMessage { Message = "subscribe!" });

                    await WaitFor(() => lastReceived != null);
                }

                Assert.NotNull(lastReceived);
                Assert.Equal("subscribe!", lastReceived.Message);
            }
        }
    }
}
