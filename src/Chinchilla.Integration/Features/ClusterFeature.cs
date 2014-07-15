using Chinchilla.Integration.Features.Consumers;
using Chinchilla.Integration.Features.Messages;
using NUnit.Framework;

namespace Chinchilla.Integration.Features
{
    [TestFixture]
    public class ClusterFeature : WithApi
    {
        [Test, Explicit]
        public void ShouldVisitTopologyWithQueueBoundToExchange()
        {
            using (var bus = Depot.Connect("localhost/integration2"))
            {
                using (var subscriber = bus.Subscribe(new HelloWorldMessageConsumer()))
                {
                    for (var i = 0; i < 100; ++i)
                    {
                        bus.Publish(new HelloWorldMessage
                        {
                            Message = "subscribe!"
                        });
                    }

                    WaitForDelivery();

                    Assert.That(subscriber.State.TotalAcceptedMessages(), Is.EqualTo(100));
                }
            }
        }
    }
}
