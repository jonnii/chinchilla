using System.Threading;
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

                    Thread.Sleep(1000);

                    Assert.That(subscriber.NumAcceptedMessages, Is.EqualTo(100));
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

                    Thread.Sleep(1000);

                    Assert.That(subscriber.NumAcceptedMessages, Is.EqualTo(100));
                }
            }
        }
    }
}
