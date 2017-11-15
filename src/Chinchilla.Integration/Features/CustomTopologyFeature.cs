using System;
using System.Threading;
using System.Threading.Tasks;
using Chinchilla.Configuration;
using Chinchilla.Integration.Features.Messages;
using Chinchilla.Topologies;
using Chinchilla.Topologies.Model;
using Xunit;

namespace Chinchilla.Integration.Features
{
    public class CustomTopologyFeature : Feature
    {
        [Fact]
        public async Task ShouldSubscribeWithCustomTopology()
        {
            using (var bus = await CreateBus())
            {
                HelloWorldMessage lastReceived = null;
                var numReceived = 0;

                var onMessage = new Action<HelloWorldMessage>(hwm => { lastReceived = hwm; Interlocked.Increment(ref numReceived); });
                var subscriptionBuilder = new Action<ISubscriptionBuilder>(b =>
                    b.SetTopology(new CustomSubscribeMessageTopology())
                );

                var publisherBuilder = new Action<IPublisherBuilder<HelloWorldMessage>>(b =>
                    b.SetTopology(new CustomSubscribeMessageTopology())
                );

                using (bus.Subscribe(onMessage, subscriptionBuilder))
                {
                    using (var publisher = bus.CreatePublisher(publisherBuilder))
                    {
                        for (var i = 0; i < 100; ++i)
                        {
                            publisher.Publish(new HelloWorldMessage { Message = i % 2 == 0 ? "even" : "odd" });
                        }
                    }

                    await WaitFor(() => lastReceived != null);
                }

                Assert.NotNull(lastReceived);
                Assert.Equal("even", lastReceived.Message);
                Assert.Equal(50, numReceived);
            }
        }

        public class CustomSubscribeMessageTopology : IMessageTopologyBuilder
        {
            public IMessageTopology Build(IEndpoint endpoint)
            {
                var topology = new MessageTopology();

                var exchange = topology.DefineExchange(endpoint.MessageType, ExchangeType.Topic);

                topology.SubscribeQueue = topology.DefineQueue(endpoint.MessageType);
                topology.SubscribeQueue.BindTo(exchange, "messages.even");

                topology.PublishExchange = exchange;

                return topology;
            }
        }
    }
}
