using System;
using System.Linq;
using Chinchilla.Integration.Features.Messages;
using NUnit.Framework;

namespace Chinchilla.Integration.Features
{
    [TestFixture]
    public class StateQueryFeature : WithApi
    {
        [Test]
        public void ShouldCreateSubscriberWithWorkerPoolStrategy()
        {
            using (var bus = Depot.Connect("localhost/integration"))
            {
                var handler = new Action<HelloWorldMessage>(hwm => { });

                var subscription = bus.Subscribe(handler, o => o.DeliverUsing<WorkerPoolDeliveryStrategy>(s => s.NumWorkers = 5));

                WaitForDelivery();

                var state = subscription.GetState();

                var queueState = state.QueueStates.Single();

                Assert.That(queueState.NumAcceptedMessages, Is.EqualTo(0));
                Assert.That(queueState.NumFailedMessages, Is.EqualTo(0));

                var workerStates = state.WorkerStates;

                Assert.That(workerStates.Length, Is.EqualTo(5));
                foreach (var workerState in workerStates)
                {
                    Assert.That(workerState.Status, Is.EqualTo(WorkerStatus.Idle));
                }

                subscription.Dispose();

                state = subscription.GetState();
                workerStates = state.WorkerStates;
                Assert.That(workerStates.Length, Is.EqualTo(5));
                foreach (var workerState in workerStates)
                {
                    Assert.That(workerState.Status, Is.EqualTo(WorkerStatus.Stopped));
                }
            }
        }
    }
}
