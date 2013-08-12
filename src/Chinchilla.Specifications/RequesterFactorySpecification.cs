using System;
using Chinchilla.Configuration;
using Chinchilla.Specifications.Messages;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class RequesterFactorySpecification
    {
        [Subject(typeof(RequesterFactory))]
        public class when_creating_requester : WithSubject<RequesterFactory>
        {
            Establish context = () =>
            {
                subscription = An<ISubscription>();
                subscription.WhenToldTo(s => s.Queues).Return(new[] { An<IDeliveryQueue>() });

                bus = An<IBus>();
                bus.WhenToldTo(b => b.Subscribe(Param.IsAny<Action<TestResponseMessage, IDeliveryContext>>(), Param.IsAny<Action<ISubscriptionBuilder>>()))
                   .Return(subscription);
            };

            Because of = () =>
                requester = (Requester<TestRequestMessage, TestResponseMessage>)
                    Subject.Create<TestRequestMessage, TestResponseMessage>(bus);

            It should_track_requester = () =>
                Subject.IsTracking(requester).ShouldBeTrue();

            It should_create_publisher_for_request_type = () =>
                bus.WasToldTo(b => b.CreatePublisher(Param.IsAny<Action<IPublisherBuilder<TestRequestMessage>>>()));

            static Requester<TestRequestMessage, TestResponseMessage> requester;

            static IBus bus;

            static ISubscription subscription;
        }
    }
}
