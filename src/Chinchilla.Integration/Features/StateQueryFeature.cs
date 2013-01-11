using System;
using System.Threading;
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

                using (var subscription = bus.Subscribe(handler, o => o.DeliverUsing<WorkerPoolDeliveryStrategy>(s => s.NumWorkers = 5)))
                {
                    Thread.Sleep(1000);

                    var state = subscription.GetState();

                    Assert.That(state.NumAcceptedMessages, Is.EqualTo(0));
                    Assert.That(state.NumFailedMessages, Is.EqualTo(0));
                    Assert.That(state.WorkerStates.Length, Is.EqualTo(5));

                    var workerStates = state.WorkerStates;

                    foreach (var workerState in workerStates)
                    {
                        Assert.That(workerState.Status, Is.EqualTo(WorkerStatus.Idle));
                    }
                }
            }
        }
    }
}
