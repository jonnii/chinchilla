using System;
using System.Threading;
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
                    b.SetTopology(messageType => new CustomSubscribeTopology(messageType))
                );

                var publisherBuilder = new Action<IPublisherBuilder>(b =>
                    b.SetTopology(messageType => new CustomSubscribeTopology(messageType))
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

                    Thread.Sleep(1000);
                }

                Assert.That(lastReceived, Is.Not.Null);
                Assert.That(lastReceived.Message, Is.EqualTo("even"));
                Assert.That(numReceived, Is.EqualTo(50));
            }
        }

        public class CustomSubscribeTopology : Topology, ISubscriberTopology, IPublisherTopology
        {
            public CustomSubscribeTopology(string messageType)
            {
                PublishExchange = DefineExchange(messageType, ExchangeType.Topic);

                SubscribeQueue = DefineQueue(messageType);
                SubscribeQueue.BindTo(PublishExchange, new[] { "messages.even" });
            }

            public IQueue SubscribeQueue { get; private set; }

            public IExchange PublishExchange { get; private set; }
        }
    }
}
