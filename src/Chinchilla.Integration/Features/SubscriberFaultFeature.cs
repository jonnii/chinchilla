using System;
using System.Linq;
using Chinchilla.Api;
using Chinchilla.Integration.Features.Messages;
using NUnit.Framework;

namespace Chinchilla.Integration.Features
{
    [TestFixture]
    public class SubscriberFaultFeature : WithApi
    {
        [Test]
        public void ShouldAllowConfigurationOfCustomDeliveryFailureStrategies()
        {
            using (var bus = Depot.Connect("localhost/integration"))
            {
                var numExceptions = 0;

                bus.Subscribe(
                    (HelloWorldMessage hwm) => { throw new Exception("ERMAGHERD, EXPLODE!!11"); },
                    opt => opt.OnFailure<CustomSubscriptionFailureStrategy>(s => s.OnException(() => ++numExceptions)));

                bus.Publish(new HelloWorldMessage { Message = "subscribe!" });

                WaitForDelivery();

                Assert.That(numExceptions, Is.EqualTo(1));
            }
        }

        [Test]
        public void ShouldHandleExceptionsWithDefaultExceptionHandler()
        {
            using (var bus = Depot.Connect("localhost/integration"))
            {
                bus.Subscribe((HelloWorldMessage hwm) => { throw new Exception("ERMAGHERD, EXPLODE!!11"); });
                bus.Publish(new HelloWorldMessage { Message = "subscribe!" });

                WaitForDelivery();

                Assert.That(admin.Exists(IntegrationVHost, new Queue("ErrorQueue")));
                Assert.That(admin.Exists(IntegrationVHost, new Exchange("ErrorExchange")));

                var messages = admin.Messages(IntegrationVHost, new Queue("ErrorQueue"));
                Assert.That(messages.Count(), Is.EqualTo(1));
            }
        }

        public class CustomSubscriptionFailureStrategy : ISubscriptionFailureStrategy
        {
            private Action notifier;

            public void OnFailure(IDelivery delivery, Exception exception)
            {
                notifier();
            }

            public void OnException(Action act)
            {
                notifier = act;
            }
        }
    }
}
