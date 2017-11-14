// using System.Linq;
// using Chinchilla.Integration.Features.Messages;
// using NUnit.Framework;

// namespace Chinchilla.Integration.Features
// {
//     [TestFixture]
//     public class PublisherConfirmsFeature : WithApi
//     {
//         [Test]
//         public void ShouldCreatePublisherWithoutConfirms()
//         {
//             using (var bus = Depot.Connect("localhost/integration"))
//             {
//                 var publisher = bus.CreatePublisher<HelloWorldMessage>(p => p.Confirm(false));

//                 var receipts = Enumerable.Range(0, 100)
//                     .Select(_ => publisher.Publish(new HelloWorldMessage()))
//                     .ToArray();

//                 publisher.Dispose();

//                 Assert.That(receipts.All(r => r.Status == PublishStatus.None));
//             }
//         }

//         [Test]
//         public void ShouldWaitForAllMessagesToBeConfirmedWhenDisposing()
//         {
//             using (var bus = Depot.Connect("localhost/integration"))
//             {
//                 var publisher = bus.CreatePublisher<HelloWorldMessage>(p => p.Confirm(true));

//                 var receipts = Enumerable.Range(0, 100)
//                     .Select(_ => publisher.Publish(new HelloWorldMessage()))
//                     .ToArray();

//                 publisher.Dispose();

//                 Assert.That(receipts.All(r => r.IsConfirmed));
//             }
//         }
//     }
// }
