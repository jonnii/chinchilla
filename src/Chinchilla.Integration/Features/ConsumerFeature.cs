using Chinchilla.Api;
using Chinchilla.Integration.Features.Consumers;
using Chinchilla.Integration.Features.Messages;
using NUnit.Framework;

namespace Chinchilla.Integration.Features
{
    [TestFixture]
    public class ConsumerFeature : WithApi
    {
        [Test]
        public void ShouldSubscribeWithConsumerInstance()
        {
            using (var bus = Depot.Connect("localhost/integration"))
            {
                using (var subscriber = bus.Subscribe(new HelloWorldMessageConsumer()))
                {
                    for (var i = 0; i < 100; ++i)
                    {
                        bus.Publish(new HelloWorldMessage { Message = "subscribe!" });
                    }

                    WaitForDelivery();

                    Assert.That(subscriber.GetState().TotalAcceptedMessages(), Is.EqualTo(100));
                }
            }
        }

        [Test]
        public void ShouldSubscribeWithConsumerType()
        {
            using (var bus = Depot.Connect("localhost/integration"))
            {
                using (var subscriber = bus.Subscribe<HelloWorldMessageConsumer>())
                {
                    for (var i = 0; i < 100; ++i)
                    {
                        bus.Publish(new HelloWorldMessage { Message = "subscribe!" });
                    }

                    WaitForDelivery();

                    Assert.That(subscriber.GetState().TotalAcceptedMessages(), Is.EqualTo(100));
                }
            }
        }

        [Test]
        public void ShouldSubscribeWithConsumerWithCustomConfiguration()
        {
            using (var bus = Depot.Connect("localhost/integration"))
            {
                using (bus.Subscribe<CustomConfigurationConsumer>())
                {
                    for (var i = 0; i < 1; ++i)
                    {
                        bus.Publish(new HelloWorldMessage { Message = "subscribe!" });
                    }

                    WaitForDelivery();

                    Assert.That(admin.Exists(IntegrationVHost, new Queue("custom-subscription-endpoint")), "did not create queue");
                }
            }
        }
    }
}
