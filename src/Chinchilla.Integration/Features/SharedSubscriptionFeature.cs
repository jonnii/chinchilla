using System.Threading;
using Chinchilla.Integration.Features.Messages;
using Chinchilla.Topologies;
using Chinchilla.Topologies.Model;
using NUnit.Framework;

namespace Chinchilla.Integration.Features
{
    [TestFixture]
    public class SharedSubscriptionFeature : WithApi
    {
        [Test]
        public void ShouldReceivedPublishedMessage()
        {
            using (var bus = Depot.Connect("localhost/integration"))
            {
                var numReceived = 0;

                bus.Subscribe((HelloWorldMessage hwm) =>
                    Interlocked.Increment(ref numReceived),
                        o => o.SubscribeOn("queue-1", "queue-2")
                              .SetTopology(new SharedSubscriptionTopology()));

                using (var first = bus.CreatePublisher<HelloWorldMessage>(o => o.PublishOn("exchange-queue-1")))
                using (var second = bus.CreatePublisher<HelloWorldMessage>(o => o.PublishOn("exchange-queue-2")))
                {
                    for (var i = 0; i < 25; ++i)
                    {
                        first.Publish(new HelloWorldMessage { Message = "subscribe!" });
                        second.Publish(new HelloWorldMessage { Message = "subscribe!" });
                    }
                }

                WaitForDelivery();

                Assert.That(numReceived, Is.EqualTo(50));
            }
        }

        public class SharedSubscriptionTopology : IMessageTopologyBuilder
        {
            public IMessageTopology Build(IEndpoint endpoint)
            {
                var topology = new MessageTopology();

                var exchange = topology.DefineExchange("exchange-" + endpoint.Name, ExchangeType.Topic);

                topology.SubscribeQueue = topology.DefineQueue(endpoint.Name);
                topology.SubscribeQueue.BindTo(exchange);

                topology.PublishExchange = exchange;

                return topology;
            }
        }
    }
}
