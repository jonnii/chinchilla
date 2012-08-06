using Chinchilla.Specifications.Messages;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class SubscriptionSpecification
    {
        [Subject(typeof(Subscription<>))]
        public class when_starting : WithSubject<Subscription<TestMessage>>
        {
            Because of = () =>
                Subject.Start();

            It should_start_delivery_strategy = () =>
                The<IDeliveryStrategy>().WasToldTo(d => d.Start());
        }

        [Subject(typeof(Subscription<>))]
        public class when_disposing : WithSubject<Subscription<TestMessage>>
        {
            Because of = () =>
                Subject.Dispose();

            It should_dispose_delivery_strategy = () =>
                The<IDeliveryStrategy>().WasToldTo(d => d.Dispose());
        }
    }
}
