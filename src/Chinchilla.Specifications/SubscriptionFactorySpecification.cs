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
                    new Subscription(An<IModelReference>(), An<IDeliveryStrategy>(), An<IFaultStrategy>(), An<IEnumerable<IQueue>>()));

            It should_track_subscription = () =>
                Subject.IsTracking(subscription).ShouldBeTrue();

            static Subscription subscription;
        }

        [Subject(typeof(SubscriptionFactory))]
        public class when_disposing_of_subscription : WithSubject<SubscriptionFactory>
        {
            Establish context = () =>
                subscription = (Subscription)Subject.Create(
                    new Subscription(An<IModelReference>(), An<IDeliveryStrategy>(), An<IFaultStrategy>(), An<IEnumerable<IQueue>>()));

            Because of = () =>
                subscription.Dispose();

            It should_untrack_reference = () =>
                Subject.IsTracking(subscription).ShouldBeFalse();

            static Subscription subscription;
        }
    }
}
