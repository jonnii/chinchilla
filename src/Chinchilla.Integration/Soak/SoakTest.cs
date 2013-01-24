using System;
using Chinchilla.Api;
using Chinchilla.Integration.Features.Messages;
using NUnit.Framework;

namespace Chinchilla.Integration.Soak
{
    public class SoakTest : WithLogging
    {
        public readonly VirtualHost SoakVHost = new VirtualHost("soak");

        protected IRabbitAdmin admin;

        [SetUp]
        public void ResetVirtualHost()
        {
            admin = new RabbitAdmin("http://localhost:55672/api");
            admin.Delete(SoakVHost);
            admin.Create(SoakVHost);
            admin.Create(SoakVHost, new User("guest"), Permission.All);
        }

        [Test, Explicit]
        public void Should()
        {
            if (!admin.Create(SoakVHost, new Exchange("HelloWorldMessage")))
            {
                throw new Exception("Could not create exchange");
            }

            if (!admin.Create(SoakVHost, new Queue("sink")))
            {
                throw new Exception("Could not create queue");
            }

            if (!admin.Create(SoakVHost, new Exchange("HelloWorldMessage"), new Queue("sink")))
            {
                throw new Exception("Could not create binding");
            }

            using (var bus = Depot.Connect("localhost/soak"))
            {
                bus.Publish(new HelloWorldMessage());
            }
        }
    }
}
