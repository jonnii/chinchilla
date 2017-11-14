// using System.Linq;
// using System.Threading;
// using Chinchilla.Integration.Features.Messages;
// using NUnit.Framework;

// namespace Chinchilla.Integration.Features
// {
//     [TestFixture]
//     public class PausableWorkersFeature : WithApi
//     {
//         [Test]
//         public void ShouldBeAbleToPauseWorker()
//         {
//             using (var bus = Depot.Connect("localhost/integration"))
//             {
//                 var seen = 0;

//                 var subscription = bus.Subscribe<HelloWorldMessage>(
//                     m => Interlocked.Increment(ref seen),
//                     c => c.DeliverUsing<WorkerPoolDeliveryStrategy>());

//                 // pause the worker
//                 var state = subscription.State;
//                 var worker = state.Workers.Single();
//                 subscription.Workers.Pause(worker.Name);

//                 // publish a message to the queue
//                 bus.Publish(new HelloWorldMessage());
//                 WaitForDelivery();

//                 // we shouldn't have processed the message and the worker should be paused
//                 Assert.That(seen, Is.EqualTo(0));
//                 state = subscription.State;
//                 Assert.That(state.Workers.First().Status, Is.EqualTo(WorkerStatus.Paused));

//                 // resume the worker
//                 subscription.Workers.Resume(worker.Name);
//                 WaitForDelivery();
//                 Assert.That(seen, Is.EqualTo(1));
//                 state = subscription.State;
//                 Assert.That(state.Workers.First().Status, Is.Not.EqualTo(WorkerStatus.Paused));
//             }
//         }
//     }
// }
