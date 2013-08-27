using System;
using System.Linq;
using System.Threading;
using Chinchilla.Api;
using Chinchilla.Integration.Features.Messages;
using NUnit.Framework;

namespace Chinchilla.Integration.Features
{
    [TestFixture]
    public class SubscribeRejectFeature : WithApi
    {
        [Test]
        public void ShouldRequeueRejectedMessages()
        {
            int receivedMessages = 0;
            using (var bus = Depot.Connect("localhost/integration"))
            using (bus.Subscribe((HelloWorldMessage hwm, IDeliveryContext ctx) => { receivedMessages++; throw new InvalidOperationException(); },
                b => b.DeliverUsing<WorkerPoolDeliveryStrategy>(s => s.NumWorkers = 1)
                      .OnFailure<RejectMessageFailureStrategy>()
                      .WithPrefetchCount(1)))
            {
                bus.Publish(new HelloWorldMessage { Message = "subscribe!" });

                WaitForDelivery();

                var messages = admin.Messages(IntegrationVHost, new Queue("HelloWorldMessage"));
                Assert.That(receivedMessages, Is.EqualTo(1));
                Assert.That(messages.Count(), Is.EqualTo(1));
            }
        }

        public class RejectMessageFailureStrategy : ISubscriptionFailureStrategy
        {
            public void OnFailure(IDelivery delivery, Exception exception)
            {
                delivery.Reject(true);
                Thread.Sleep(5000);
            }
        }
    }
}
