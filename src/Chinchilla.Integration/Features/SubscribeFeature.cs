// using System;
// using System.Linq;
// using System.Threading;
// using Chinchilla.Api;
// using Chinchilla.Integration.Features.Messages;
// using Chinchilla.Integration.Features.MessageTypeFactories;
// using Chinchilla.Serializers;
// using NUnit.Framework;

// namespace Chinchilla.Integration.Features
// {
//     [TestFixture]
//     public class SubscribeFeature : WithApi
//     {
//         [Test]
//         public void ShouldSubscribeMessage()
//         {
//             using (var bus = Depot.Connect("localhost/integration"))
//             {
//                 bus.Subscribe((HelloWorldMessage hwm) => { });

//                 Assert.That(admin.Exists(IntegrationVHost, new Queue("HelloWorldMessage")), "did not create queue");
//                 Assert.That(admin.Exists(IntegrationVHost, new Exchange("HelloWorldMessage")), "did not create exchange");
//             }
//         }

//         [Test]
//         public void ShouldFindSubscriptionByName()
//         {
//             using (var bus = Depot.Connect("localhost/integration"))
//             {
//                 var existing = bus.Subscribe((HelloWorldMessage hwm) => { });

//                 var subscription = bus.FindSubscription(existing.Name);

//                 Assert.That(subscription, Is.Not.Null);
//             }
//         }

//         [Test]
//         public void ShouldReceivedPublishedMessage()
//         {
//             using (var bus = Depot.Connect("localhost/integration"))
//             {
//                 HelloWorldMessage lastReceived = null;
//                 var numReceived = 0;

//                 bus.Subscribe((HelloWorldMessage hwm) =>
//                 {
//                     lastReceived = hwm;
//                     Interlocked.Increment(ref numReceived);
//                 });

//                 for (var i = 0; i < 100; ++i)
//                 {
//                     bus.Publish(new HelloWorldMessage { Message = "subscribe!" });
//                 }

//                 WaitForDelivery();

//                 Assert.That(lastReceived, Is.Not.Null);
//                 Assert.That(lastReceived.Message, Is.EqualTo("subscribe!"));
//                 Assert.That(numReceived, Is.EqualTo(100));
//             }
//         }

//         [Test]
//         public void ShouldSubscribeToMessageInterfaces()
//         {
//             using (var bus = Depot.Connect("localhost/integration"))
//             {
//                 IHelloWorldMessage lastReceived = null;
//                 bus.Subscribe((IHelloWorldMessage hwm) =>
//                 {
//                     lastReceived = hwm;
//                 });
//                 bus.Publish(new HelloWorldMessage { Message = "subscribe!" });

//                 WaitForDelivery();

//                 Assert.That(lastReceived, Is.Not.Null);
//                 Assert.That(lastReceived.Message, Is.EqualTo("subscribe!"));

//                 Assert.That(admin.Exists(IntegrationVHost, new Queue("HelloWorldMessage")), "did not create queue");
//                 Assert.That(admin.Exists(IntegrationVHost, new Exchange("HelloWorldMessage")), "did not create exchange");
//             }
//         }

//         [Test]
//         public void ShouldSubscribeToMessageInterfacesWithCustomMessageFactory()
//         {
//             var serializer = new JsonMessageSerializer(
//                 new CastleMessageTypeFactory());

//             var depotSettings = new DepotSettings();
//             depotSettings.MessageSerializers.Register(serializer);

//             using (var bus = Depot.Connect("localhost/integration", depotSettings))
//             {
//                 IHelloWorldMessage lastReceived = null;
//                 bus.Subscribe((IHelloWorldMessage hwm) =>
//                 {
//                     lastReceived = hwm;
//                 });
//                 bus.Publish(new HelloWorldMessage { Message = "subscribe!" });

//                 WaitForDelivery();

//                 Assert.That(lastReceived, Is.Not.Null);
//                 Assert.That(lastReceived.Message, Is.EqualTo("subscribe!"));

//                 Assert.That(admin.Exists(IntegrationVHost, new Queue("HelloWorldMessage")), "did not create queue");
//                 Assert.That(admin.Exists(IntegrationVHost, new Exchange("HelloWorldMessage")), "did not create exchange");
//             }
//         }

//         [Test]
//         public void ShouldCreateSubscriberWithWorkerPoolStrategy()
//         {
//             using (var bus = Depot.Connect("localhost/integration"))
//             {
//                 HelloWorldMessage lastReceived = null;
//                 var numReceived = 0;
//                 var handler = new Action<HelloWorldMessage>(hwm => { lastReceived = hwm; ++numReceived; });

//                 bus.Subscribe(handler, o => o.DeliverUsing<WorkerPoolDeliveryStrategy>(s => s.NumWorkers = 5));

//                 for (var i = 0; i < 100; ++i)
//                 {
//                     bus.Publish(new HelloWorldMessage { Message = "subscribe!" });
//                 }

//                 WaitForDelivery();

//                 Assert.That(lastReceived, Is.Not.Null);
//                 Assert.That(lastReceived.Message, Is.EqualTo("subscribe!"));
//                 Assert.That(numReceived, Is.EqualTo(100));
//             }
//         }

//         [Test]
//         public void ShouldNotStartSubscriptionIfSubscriptionConfigurationIsInvalidAndThrowsIfStarted()
//         {
//             using (var bus = Depot.Connect("localhost/integration"))
//             {
//                 var subscription = bus.Subscribe(
//                     (HelloWorldMessage m) => { },
//                     o => o.DeliverUsing<WorkerPoolDeliveryStrategy>(s => s.NumWorkers = 0));

//                 Assert.That(subscription.IsStarted, Is.False);
//                 Assert.Throws<ChinchillaException>(subscription.Start);
//             }
//         }

//         [Test]
//         public void ShouldCreateSubscriberWithTaskPoolStrategy()
//         {
//             using (var bus = Depot.Connect("localhost/integration"))
//             {
//                 HelloWorldMessage lastReceived = null;
//                 var numReceived = 0;
//                 var handler = new Action<HelloWorldMessage>(hwm => { lastReceived = hwm; ++numReceived; });

//                 using (var subscription = bus.Subscribe(handler, o => o.DeliverUsing<TaskDeliveryStrategy>()))
//                 {
//                     for (var i = 0; i < 100; ++i)
//                     {
//                         bus.Publish(new HelloWorldMessage
//                         {
//                             Message = "subscribe!"
//                         });
//                     }

//                     WaitForDelivery();

//                     Assert.That(lastReceived, Is.Not.Null);
//                     Assert.That(lastReceived.Message, Is.EqualTo("subscribe!"));
//                     Assert.That(numReceived, Is.EqualTo(100));

//                     Assert.That(subscription.State.Workers.Count(), Is.EqualTo(0));
//                 }
//             }
//         }

//         [Test]
//         public void ShouldCreateSubscriberWithSpecificSubscriptionQueue()
//         {
//             using (var bus = Depot.Connect("localhost/integration"))
//             {
//                 var subscription = bus.Subscribe((HelloWorldMessage m) => { }, o => o.SubscribeOn("gimme-dem-messages"));

//                 var queueName = subscription.Queues.Single().Name;
//                 Assert.That(queueName, Is.EqualTo("gimme-dem-messages"));
//             }
//         }

//         [Test]
//         public void ShouldCreateSubscriptionWithName()
//         {
//             using (var bus = Depot.Connect("localhost/integration"))
//             {
//                 var subscription = bus.Subscribe((HelloWorldMessage m) => { }, o => o.WithName("fribble"));
//                 Assert.That(subscription.Name, Is.EqualTo("fribble"));
//             }
//         }

//         [Test]
//         public void ShouldCreateSubscriberWithCallbackWithContext()
//         {
//             using (var bus = Depot.Connect("localhost/integration"))
//             {
//                 var numReceived = 0;

//                 bus.Subscribe<HelloWorldMessage>((hwm, ctx) =>
//                 {
//                     ++numReceived;
//                     Assert.That(ctx, Is.Not.Null);
//                 });
//                 for (var i = 0; i < 100; ++i)
//                 {
//                     bus.Publish(new HelloWorldMessage { Message = "subscribe!" });
//                 }

//                 WaitForDelivery();

//                 Assert.That(numReceived, Is.EqualTo(100));
//             }
//         }

//         [Test]
//         public void ShouldRunAdditionalRegisteredListeners()
//         {
//             using (var bus = Depot.Connect("localhost/integration"))
//             {
//                 var didRunAfterAccept = false;

//                 bus.Subscribe<HelloWorldMessage>((hwm, ctx) =>
//                     ctx.Delivery.RegisterDeliveryListener(new ActionDeliveryListener(() => didRunAfterAccept = true)));

//                 bus.Publish(new HelloWorldMessage { Message = "subscribe!" });

//                 WaitForDelivery();

//                 Assert.That(didRunAfterAccept, Is.True);
//             }
//         }

//         [Test]
//         public void ShouldShutdown()
//         {
//             var seen = 0;

//             using (var bus = Depot.Connect("localhost/integration"))
//             {
//                 var subscription = bus.Subscribe((HelloWorldMessage hwm) =>
//                 {
//                     Console.WriteLine("!!! Starting message");
//                     WaitForDelivery();
//                     ++seen;
//                     Console.WriteLine("!!! Finished message");
//                 }, o => o.DeliverUsing<WorkerPoolDeliveryStrategy>(s => s.NumWorkers = 1));

//                 bus.Publish(new HelloWorldMessage
//                 {
//                     Message = "subscribe!"
//                 });

//                 WaitForDelivery();

//                 subscription.Dispose();
//             }

//             Assert.That(seen, Is.EqualTo(1));
//             var count = admin.Messages(IntegrationVHost, new Queue("HelloWorldMessage")).Count();
//             Assert.That(count, Is.EqualTo(0));
//         }
//     }
// }
