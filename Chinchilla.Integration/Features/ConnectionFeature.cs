using System;
using Chinchilla.Api;
using NUnit.Framework;
using RabbitMQ.Client;

namespace Chinchilla.Integration.Features
{
    [TestFixture]
    public class ConnectionFeature
    {
        [SetUp]
        public void ResetVirtualHost()
        {
            var api = RabbitAdmin.Create("http://localhost:55672/api");
            api.Delete(new VirtualHost("integration"));
            api.Create(new VirtualHost("integration"));
            api.Create(new VirtualHost("integration"), new User("guest"), Permissions.All);
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
        public void ShouldVisit()
        {
            var factory = new ConnectionFactoryAdapter();
            var connection = factory.Create(new Uri("amqp://localhost/integration"));

            var model = connection.CreateModel();

            Assert.That(model.IsOpen);

            model.ExchangeDeclare("exchange-name", "fanout", true);
            model.QueueDeclare("test-queue", true, true, false, null);
            model.QueueBind("test-queue", "exchange-name", "#");
        }
    }
}
