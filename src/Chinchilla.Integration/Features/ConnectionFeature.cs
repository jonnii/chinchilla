using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Chinchilla.Api;
using Chinchilla.Integration.Features.Messages;
using Chinchilla.Topologies.Model;
using Xunit;
using ExchangeType = Chinchilla.Topologies.Model.ExchangeType;

namespace Chinchilla.Integration.Features
{
    public class ConnectionFeature : Feature
    {
        [Fact]
        public async Task ShouldVisitTopologyWithQueueBoundToExchange()
        {
            var vhost = await CreateVHost();
            var factory = new DefaultConnectionFactory();

            using (var connection = factory.Create(new Uri($"amqp://localhost/{vhost}")))
            {
                var model = connection.CreateModel();

                var topology = new Topology();
                var e1 = topology.DefineExchange("exchange1", ExchangeType.Topic);
                var q1 = topology.DefineQueue("queue");

                q1.BindTo(e1);

                topology.Visit(new TopologyBuilder(model));

                var exchanges = await Admin.ExchangesAsync(new VirtualHost(vhost));
                Assert.Contains(e1.Name, exchanges.Select(e => e.Name));

                var queues = await Admin.QueuesAsync(new VirtualHost(vhost));
                Assert.Contains(q1.Name, queues.Select(e => e.Name));
            }
        }

        [Fact]
        public async Task ShouldVisitExclusiveQueue()
        {
            var vhost = await CreateVHost();
            var factory = new DefaultConnectionFactory();

            using (var connection = factory.Create(new Uri($"amqp://localhost/{vhost}")))
            {
                var model = connection.CreateModel();

                var topology = new Topology();
                var q1 = topology.DefineQueue();

                topology.Visit(new TopologyBuilder(model));

                Assert.True(q1.HasName);

                var queues = await Admin.QueuesAsync(new VirtualHost(vhost));
                Assert.Contains(q1.Name, queues.Select(e => e.Name).ToArray());
            }
        }

        [Fact]
        public async Task ShouldVisitTopologyMultipleTimesWithoutExceptions()
        {
            var vhost = await CreateVHost();
            var factory = new DefaultConnectionFactory();

            using (var connection = factory.Create(new Uri($"amqp://localhost/{vhost}")))
            {
                var model = connection.CreateModel();

                var topology = new Topology();
                topology.DefineQueue("test-queue");

                var builder = new TopologyBuilder(model);

                topology.Visit(builder);
                topology.Visit(builder);
            }
        }

        [Fact]
        public async Task ShouldVisitTopologyMultipleTimesExclusiveQueue()
        {
            var vhost = await CreateVHost();
            var factory = new DefaultConnectionFactory();

            using (var connection = factory.Create(new Uri($"amqp://localhost/{vhost}")))
            {
                var model = connection.CreateModel();

                var topology = new Topology();
                topology.DefineQueue();

                var builder = new TopologyBuilder(model);
                topology.Visit(builder);
                topology.Visit(builder);
            }
        }

        [Fact]
        public async Task ShouldSurviveBeingDisconnected()
        {
            var vhost = await CreateVHost();

            using (var bus = Depot.Connect($"localhost/{vhost}"))
            {
                var numReceived = 0;
                var handler = new Action<HelloWorldMessage>(hwm =>
                {
                    Interlocked.Increment(ref numReceived);

                    if (numReceived == 50)
                    {
                        var connections = Admin.ConnectionsAsync().Result;
                        Admin.DeleteAsync(connections.First()).Wait();
                    }
                });

                var subscription = bus.Subscribe(handler);

                using (subscription)
                {
                    var publisher = bus.CreatePublisher<HelloWorldMessage>();
                    for (var i = 0; i < 100; ++i)
                    {
                        publisher.Publish(new HelloWorldMessage { Message = "subscribe!" });
                    }

                    await WaitFor(() => numReceived >= 100);
                }

                Assert.True(numReceived >= 100);
            }
        }
    }
}
