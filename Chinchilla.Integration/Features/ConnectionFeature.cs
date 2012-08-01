using System;
using System.Linq;
using Chinchilla.Api;
using Chinchilla.Topologies.Rabbit;
using NUnit.Framework;
using RabbitMQ.Client;
using ExchangeType = Chinchilla.Topologies.Rabbit.ExchangeType;

namespace Chinchilla.Integration.Features
{
    [TestFixture]
    public class ConnectionFeature
    {
        private IRabbitAdmin admin;

        [SetUp]
        public void ResetVirtualHost()
        {
            admin = RabbitAdmin.Create("http://localhost:55672/api");
            admin.Delete(new VirtualHost("integration"));
            admin.Create(new VirtualHost("integration"));
            admin.Create(new VirtualHost("integration"), new User("guest"), Permissions.All);
        }

        [Test]
        public void ShouldConnectWithUri()
        {
            var factory = new ConnectionFactory();
            var adapter = new ConnectionFactoryAdapter(factory);

            var connection = adapter.Create(new Uri("amqp://localhost"));

            Assert.That(connection.IsOpen);
        }

        [Test]
        public void ShouldVisitTopologyWithQueueBoundToExchange()
        {
            var factory = new ConnectionFactoryAdapter();
            var connection = factory.Create(new Uri("amqp://localhost/integration"));

            var model = connection.CreateModel();

            var topology = new Topology();
            var e1 = topology.DefineExchange("exchange1", ExchangeType.Topic);
            var q1 = topology.DefineQueue("queue");

            q1.BindTo(e1);

            topology.Visit(new TopologyBuilder(model));

            var exchanges = admin.Exchanges(new VirtualHost("integration"));
            Assert.That(exchanges.Any(e => e.Name == e1.Name));

            var queues = admin.Queues(new VirtualHost("integration"));
            Assert.That(queues.Any(e => e.Name == q1.Name));
        }

        [Test]
        public void ShouldVisitQueueWithoutName()
        {
            var factory = new ConnectionFactoryAdapter();
            var connection = factory.Create(new Uri("amqp://localhost/integration"));

            var model = connection.CreateModel();

            var topology = new Topology();
            var q1 = topology.DefineQueue();

            topology.Visit(new TopologyBuilder(model));

            Assert.That(q1.HasName);

            var queues = admin.Queues(new VirtualHost("integration"));
            Assert.That(queues.Any(e => e.Name == q1.Name));
        }
    }
}
