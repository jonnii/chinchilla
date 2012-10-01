using System.Collections.Generic;
using Chinchilla.Topologies.Model;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class SubscriptionFactorySpecification
    {
        [Subject(typeof(SubscriptionFactory))]
        public class when_creating_subscription : WithSubject<SubscriptionFactory>
        {
            Because of = () =>
                subscription = (Subscription)Subject.Create(
                    new Subscription(An<IDeliveryStrategy>(), new[] { An<IDeliveryQueue>() }));

            It should_track_subscription = () =>
                Subject.IsTracking(subscription).ShouldBeTrue();

            static Subscription subscription;
        }

        [Subject(typeof(SubscriptionFactory))]
        public class when_disposing_of_subscription : WithSubject<SubscriptionFactory>
        {
            Establish context = () =>
                subscription = (Subscription)Subject.Create(
                    new Subscription(An<IDeliveryStrategy>(), new[] { An<IDeliveryQueue>() }));

            Because of = () =>
                subscription.Dispose();

            It should_untrack_reference = () =>
                Subject.IsTracking(subscription).ShouldBeFalse();

            static Subscription subscription;
        }
    }
}
