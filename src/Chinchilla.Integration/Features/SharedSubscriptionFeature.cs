using System.Threading;
using System.Threading.Tasks;
using Chinchilla.Integration.Features.Messages;
using Chinchilla.Topologies;
using Chinchilla.Topologies.Model;
using Xunit;

namespace Chinchilla.Integration.Features
{
    public class SharedSubscriptionFeature : Feature
    {
        //[Fact]
        //public async Task ShouldReceivedPublishedMessage()
        //{
        //    using (var bus = await CreateBus())
        //    {
        //        var numReceived = 0;

        //        bus.Subscribe((HelloWorldMessage hwm) =>
        //            Interlocked.Increment(ref numReceived),
        //                o => o.SubscribeOn("queue-1", "queue-2")
        //                      .SetTopology(new SharedSubscriptionTopology()));

        //        using (var first = bus.CreatePublisher<HelloWorldMessage>(o => o.PublishOn("exchange-queue-1")))
        //        using (var second = bus.CreatePublisher<HelloWorldMessage>(o => o.PublishOn("exchange-queue-2")))
        //        {
        //            for (var i = 0; i < 25; ++i)
        //            {
        //                first.Publish(new HelloWorldMessage { Message = "subscribe!" });
        //                second.Publish(new HelloWorldMessage { Message = "subscribe!" });
        //            }
        //        }

        //        await WaitFor(() => numReceived == 50);

        //        Assert.Equal(50, numReceived);
        //    }
        //}

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
