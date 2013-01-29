using System;
using System.Linq;
using Chinchilla.Api;
using Chinchilla.Integration.Features.Messages;
using NUnit.Framework;

namespace Chinchilla.Integration.Features
{
    [TestFixture]
    public class SubscribeFeature : WithApi
    {
        [Test]
        public void ShouldSubscribeMessage()
        {
            using (var bus = Depot.Connect("localhost/integration"))
            {
                bus.Subscribe((HelloWorldMessage hwm) => { });

                Assert.That(admin.Exists(IntegrationVHost, new Queue("HelloWorldMessage")), "did not create queue");
                Assert.That(admin.Exists(IntegrationVHost, new Exchange("HelloWorldMessage")), "did not create exchange");
            }
        }

        [Test]
        public void ShouldReceivedPublishedMessage()
        {
            using (var bus = Depot.Connect("localhost/integration"))
            {
                HelloWorldMessage lastReceived = null;
                var numReceived = 0;

                bus.Subscribe((HelloWorldMessage hwm) =>
                {
                    lastReceived = hwm;
                    ++numReceived;
                });

                for (var i = 0; i < 100; ++i)
                {
                    bus.Publish(new HelloWorldMessage { Message = "subscribe!" });
                }

                WaitForDelivery();

                Assert.That(lastReceived, Is.Not.Null);
                Assert.That(lastReceived.Message, Is.EqualTo("subscribe!"));
                Assert.That(numReceived, Is.EqualTo(100));
            }
        }

        [Test]
        public void ShouldCreateSubscriberWithWorkerPoolStrategy()
        {
            using (var bus = Depot.Connect("localhost/integration"))
            {
                HelloWorldMessage lastReceived = null;
                var numReceived = 0;
                var handler = new Action<HelloWorldMessage>(hwm => { lastReceived = hwm; ++numReceived; });

                bus.Subscribe(handler, o => o.DeliverUsing<WorkerPoolDeliveryStrategy>(s => s.NumWorkers = 5));

                for (var i = 0; i < 100; ++i)
                {
                    bus.Publish(new HelloWorldMessage { Message = "subscribe!" });
                }

                WaitForDelivery();

                Assert.That(lastReceived, Is.Not.Null);
                Assert.That(lastReceived.Message, Is.EqualTo("subscribe!"));
                Assert.That(numReceived, Is.EqualTo(100));
            }
        }

        [Test]
        public void ShouldNotStartSubscriptionIfSubscriptionConfigurationIsInvalidAndThrowsIfStarted()
        {
            using (var bus = Depot.Connect("localhost/integration"))
            {
                var subscription = bus.Subscribe(
                    (HelloWorldMessage m) => { },
                    o => o.DeliverUsing<WorkerPoolDeliveryStrategy>(s => s.NumWorkers = 0));

                Assert.That(subscription.IsStarted, Is.False);
                Assert.Throws<ChinchillaException>(subscription.Start);
            }
        }

        [Test]
        public void ShouldCreateSubscriberWithTaskPoolStrategy()
        {
            using (var bus = Depot.Connect("localhost/integration"))
            {
                HelloWorldMessage lastReceived = null;
                var numReceived = 0;
                var handler = new Action<HelloWorldMessage>(hwm => { lastReceived = hwm; ++numReceived; });

                using (var subscription = bus.Subscribe(handler, o => o.DeliverUsing<TaskDeliveryStrategy>()))
                {
                    for (var i = 0; i < 100; ++i)
                    {
                        bus.Publish(new HelloWorldMessage
                        {
                            Message = "subscribe!"
                        });
                    }

                    WaitForDelivery();

                    Assert.That(lastReceived, Is.Not.Null);
                    Assert.That(lastReceived.Message, Is.EqualTo("subscribe!"));
                    Assert.That(numReceived, Is.EqualTo(100));

                    Assert.That(subscription.State.Workers.Count(), Is.EqualTo(0));
                }
            }
        }

        [Test]
        public void ShouldCreateSubscriberWithSpecificSubscriptionQueue()
        {
            using (var bus = Depot.Connect("localhost/integration"))
            {
                var subscription = bus.Subscribe((HelloWorldMessage m) => { }, o => o.SubscribeOn("gimme-dem-messages"));

                var queueName = subscription.Queues.Single().Name;
                Assert.That(queueName, Is.EqualTo("gimme-dem-messages"));
            }
        }

        [Test]
        public void ShouldCreateSubscriberWithCallbackWithContext()
        {
            using (var bus = Depot.Connect("localhost/integration"))
            {
                var numReceived = 0;

                bus.Subscribe<HelloWorldMessage>((hwm, ctx) =>
                {
                    ++numReceived;
                    Assert.That(ctx, Is.Not.Null);
                });
                for (var i = 0; i < 100; ++i)
                {
                    bus.Publish(new HelloWorldMessage { Message = "subscribe!" });
                }

                WaitForDelivery();

                Assert.That(numReceived, Is.EqualTo(100));
            }
        }

        [Test]
        public void ShouldShutdown()
        {
            var seen = 0;

            using (var bus = Depot.Connect("localhost/integration"))
            {
                var subscription = bus.Subscribe((HelloWorldMessage hwm) =>
                {
                    Console.WriteLine("!!! Starting message");
                    WaitForDelivery();
                    ++seen;
                    Console.WriteLine("!!! Finished message");
                }, o => o.DeliverUsing<WorkerPoolDeliveryStrategy>(s => s.NumWorkers = 1));

                bus.Publish(new HelloWorldMessage
                {
                    Message = "subscribe!"
                });

                WaitForDelivery();

                subscription.Dispose();
            }

            Assert.That(seen, Is.EqualTo(1));
            var count = admin.Messages(IntegrationVHost, new Queue("HelloWorldMessage")).Count();
            Assert.That(count, Is.EqualTo(0));
        }
    }
}
