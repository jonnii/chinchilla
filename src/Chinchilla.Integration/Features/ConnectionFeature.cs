using System;
using System.Linq;
using System.Threading;
using Chinchilla.Integration.Features.Messages;
using Chinchilla.Topologies.Model;
using NUnit.Framework;
using ExchangeType = Chinchilla.Topologies.Model.ExchangeType;

namespace Chinchilla.Integration.Features
{
    [TestFixture]
    public class ConnectionFeature : WithApi
    {
        [Test]
        public void ShouldVisitTopologyWithQueueBoundToExchange()
        {
            var factory = new DefaultConnectionFactory();
            var connection = factory.Create(new Uri("amqp://localhost/integration"));

            var model = connection.CreateModel();

            var topology = new Topology();
            var e1 = topology.DefineExchange("exchange1", ExchangeType.Topic);
            var q1 = topology.DefineQueue("queue");

            q1.BindTo(e1);

            topology.Visit(new TopologyBuilder(model));

            var exchanges = admin.Exchanges(IntegrationVHost);
            Assert.That(exchanges.Any(e => e.Name == e1.Name));

            var queues = admin.Queues(IntegrationVHost);
            Assert.That(queues.Any(e => e.Name == q1.Name));
        }

        [Test]
        public void ShouldVisitExclusiveQueue()
        {
            var factory = new DefaultConnectionFactory();
            var connection = factory.Create(new Uri("amqp://localhost/integration"));

            var model = connection.CreateModel();

            var topology = new Topology();
            var q1 = topology.DefineQueue();

            topology.Visit(new TopologyBuilder(model));

            Assert.That(q1.HasName);

            var queues = admin.Queues(IntegrationVHost);
            Assert.That(queues.Any(e => e.Name == q1.Name));
        }

        [Test]
        public void ShouldVisitTopologyMultipleTimesWithoutExceptions()
        {
            var factory = new DefaultConnectionFactory();
            var connection = factory.Create(new Uri("amqp://localhost/integration"));

            var model = connection.CreateModel();

            var topology = new Topology();
            topology.DefineQueue("test-queue");

            var builder = new TopologyBuilder(model);

            topology.Visit(builder);
            topology.Visit(builder);
        }

        [Test]
        public void ShouldVisitTopologyMultipleTimesExclusiveQueue()
        {
            var factory = new DefaultConnectionFactory();
            var connection = factory.Create(new Uri("amqp://localhost/integration"));

            var model = connection.CreateModel();

            var topology = new Topology();
            topology.DefineQueue();

            var builder = new TopologyBuilder(model);
            topology.Visit(builder);
            topology.Visit(builder);
        }

        [Test]
        public void ShouldSurviveBeingDisconnected()
        {
            using (var bus = Depot.Connect("localhost/integration"))
            {
                var numReceived = 0;
                var handler = new Action<HelloWorldMessage>(hwm =>
                {
                    ++numReceived;

                    if (numReceived == 50)
                    {
                        Console.WriteLine("Disconnecting with a vengeance");
                        var connections = admin.Connections();
                        admin.Delete(connections.First());
                    }
                });

                using (bus.Subscribe((HelloWorldMessage m) => { })) { }

                for (var i = 0; i < 100; ++i)
                {
                    bus.Publish(new HelloWorldMessage { Message = "subscribe!" });
                }

                var subscription = bus.Subscribe(handler);
                using (subscription)
                {
                    Thread.Sleep(1000);
                }

                Assert.That(numReceived, Is.GreaterThanOrEqualTo(100));
            }
        }
    }
}
