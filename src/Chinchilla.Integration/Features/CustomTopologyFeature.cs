using System;
using Chinchilla.Configuration;
using Chinchilla.Integration.Features.Messages;
using Chinchilla.Topologies;
using Chinchilla.Topologies.Model;
using NUnit.Framework;

namespace Chinchilla.Integration.Features
{
    [TestFixture]
    public class CustomTopologyFeature : WithApi
    {
        [Test]
        public void ShouldSubscribeWithCustomTopology()
        {
            using (var bus = Depot.Connect("localhost/integration"))
            {
                HelloWorldMessage lastReceived = null;
                var numReceived = 0;

                var onMessage = new Action<HelloWorldMessage>(hwm => { lastReceived = hwm; ++numReceived; });
                var subscriptionBuilder = new Action<ISubscriptionBuilder>(b =>
                    b.SetTopology(new CustomSubscribeMessageTopology())
                );

                var publisherBuilder = new Action<IPublisherBuilder>(b =>
                    b.SetTopology(new CustomSubscribeMessageTopology())
                );

                using (bus.Subscribe(onMessage, subscriptionBuilder))
                {
                    using (var publisher = bus.CreatePublisher<HelloWorldMessage>(publisherBuilder))
                    {
                        for (var i = 0; i < 100; ++i)
                        {
                            publisher.Publish(new HelloWorldMessage { Message = i % 2 == 0 ? "even" : "odd" });
                        }
                    }

                    WaitForDelivery();
                }

                Assert.That(lastReceived, Is.Not.Null);
                Assert.That(lastReceived.Message, Is.EqualTo("even"));
                Assert.That(numReceived, Is.EqualTo(50));
            }
        }

        public class CustomSubscribeMessageTopology : IMessageTopologyBuilder
        {
            public IMessageTopology Build(IEndpoint endpoint)
            {
                var topology = new MessageTopology();

                var exchange = topology.DefineExchange(endpoint.MessageType, ExchangeType.Topic);

                topology.SubscribeQueue = topology.DefineQueue(endpoint.MessageType);
                topology.SubscribeQueue.BindTo(exchange, new[] { "messages.even" });

                topology.PublishExchange = exchange;

                return topology;
            }
        }
    }
}
