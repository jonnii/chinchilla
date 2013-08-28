using System;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class SubscriptionFactorySpecification
    {
        [Subject(typeof(SubscriptionFactory))]
        public class in_general : WithSubject<SubscriptionFactory>
        {
            It should_get_message_type_name_for_class = () =>
                Subject.GetMessageTypeName(typeof(MyMessage)).ShouldEqual("MyMessage");

            It should_get_message_type_name_for_interface = () =>
                Subject.GetMessageTypeName(typeof(IMyMessage)).ShouldEqual("MyMessage");
        }

        [Subject(typeof(SubscriptionFactory))]
        public class when_creating_subscription : WithSubject<SubscriptionFactory>
        {
            Because of = () =>
                subscription = (Subscription)Subject.Create(
                    new Subscription("name", An<IModelReference>(), An<IDeliveryStrategy>(), new[] { An<IDeliveryQueue>() }));

            It should_track_subscription = () =>
                Subject.IsTracking(subscription).ShouldBeTrue();

            static Subscription subscription;
        }

        [Subject(typeof(SubscriptionFactory))]
        public class when_disposing_of_subscription : WithSubject<SubscriptionFactory>
        {
            Establish context = () =>
                subscription = (Subscription)Subject.Create(
                    new Subscription("name", An<IModelReference>(), An<IDeliveryStrategy>(), new[] { An<IDeliveryQueue>() }));

            Because of = () =>
                subscription.Dispose();

            It should_untrack_reference = () =>
                Subject.IsTracking(subscription).ShouldBeFalse();

            static Subscription subscription;
        }

        [Subject(typeof(SubscriptionFactory))]
        public class when_finding_subscription_by_name_when_no_subscription : WithSubject<SubscriptionFactory>
        {
            Because of = () =>
                exception = Catch.Exception(() => Subject.FindByName("fribble"));

            It should_throw_exception = () =>
                exception.ShouldBeOfType<ChinchillaException>();

            static Exception exception;
        }

        [Subject(typeof(SubscriptionFactory))]
        public class when_finding_subscription_by_name : WithSubject<SubscriptionFactory>
        {
            Establish context = () =>
                Subject.Create(
                    new Subscription("name", An<IModelReference>(), An<IDeliveryStrategy>(), new[] { An<IDeliveryQueue>() }));

            Because of = () =>
                subscription = Subject.FindByName("name");

            It should_find_subscription = () =>
                subscription.ShouldNotBeNull();

            static ISubscription subscription;
        }

        public interface IMyMessage { }

        public class MyMessage : IMyMessage { }
    }
}
