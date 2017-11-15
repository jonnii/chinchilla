using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Chinchilla.Api;
using Chinchilla.Integration.Features.Messages;
using Chinchilla.Topologies.Model;
using Xunit;
using ExchangeType = Chinchilla.Topologies.Model.ExchangeType;

namespace Chinchilla.Integration.Features
{
    [Collection("Rabbit Collection")]
    public class ConnectionFeature : Feature
    {
        private readonly IRabbitAdmin admin;

        private readonly VirtualHost vhost;

        public ConnectionFeature(RabbitFixture fixture)
        {
            admin = fixture.Admin;
            vhost = fixture.IntegrationVHost;
        }

        [Fact]
        public async Task ShouldVisitTopologyWithQueueBoundToExchange()
        {
            var factory = new DefaultConnectionFactory();
            using (var connection = factory.Create(new Uri("amqp://localhost/integration")))
            {
                var model = connection.CreateModel();

                var topology = new Topology();
                var e1 = topology.DefineExchange("exchange1", ExchangeType.Topic);
                var q1 = topology.DefineQueue("queue");

                q1.BindTo(e1);

                topology.Visit(new TopologyBuilder(model));

                var exchanges = await admin.ExchangesAsync(vhost);
                Assert.Contains(e1.Name, exchanges.Select(e => e.Name));

                var queues = await admin.QueuesAsync(vhost);
                Assert.Contains(q1.Name, queues.Select(e => e.Name));
            }
        }

        [Fact]
        public async Task ShouldVisitExclusiveQueue()
        {
            var factory = new DefaultConnectionFactory();
            using (var connection = factory.Create(new Uri("amqp://localhost/integration")))
            {
                var model = connection.CreateModel();

                var topology = new Topology();
                var q1 = topology.DefineQueue();

                topology.Visit(new TopologyBuilder(model));

                Assert.True(q1.HasName);

                var queues = await admin.QueuesAsync(vhost);
                Assert.Contains(q1.Name, queues.Select(e => e.Name).ToArray());
            }
        }

        [Fact]
        public void ShouldVisitTopologyMultipleTimesWithoutExceptions()
        {
            var factory = new DefaultConnectionFactory();
            using (var connection = factory.Create(new Uri("amqp://localhost/integration")))
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
        public void ShouldVisitTopologyMultipleTimesExclusiveQueue()
        {
            var factory = new DefaultConnectionFactory();
            using (var connection = factory.Create(new Uri("amqp://localhost/integration")))
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
        public void ShouldSurviveBeingDisconnected()
        {
            using (var bus = Depot.Connect("localhost/integration"))
            {
                var numReceived = 0;
                var handler = new Action<HelloWorldMessage>(async hwm =>
                {
                    Interlocked.Increment(ref numReceived);

                    if (numReceived == 50)
                    {
                        var connections = await admin.ConnectionsAsync();
                        await admin.DeleteAsync(connections.First());
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

                    WaitFor(() => numReceived >= 100);
                }

                Assert.True(numReceived >= 100);
            }
        }
    }
}
