// using System;
// using System.Linq;
// using System.Threading;
// using Chinchilla.Api;
// using Chinchilla.Integration.Features.Messages;
// using NUnit.Framework;

// namespace Chinchilla.Integration.Features
// {
//     [TestFixture]
//     public class SubscribeRejectFeature : WithApi
//     {
//         [Test]
//         public void ShouldRequeueRejectedMessages()
//         {
//             using (var bus = Depot.Connect("localhost/integration"))
//             {
//                 bus.Subscribe((HelloWorldMessage hwm) =>
//                 {
//                     throw new MessageRejectedException();
//                 });

//                 bus.Publish(new HelloWorldMessage { Message = "subscribe!" });

//                 WaitForDelivery();
//             }

//             var messages = admin.Messages(IntegrationVHost, new Queue("HelloWorldMessage"));
//             Assert.That(messages.Count(), Is.EqualTo(1));
//         }
//     }
// }
