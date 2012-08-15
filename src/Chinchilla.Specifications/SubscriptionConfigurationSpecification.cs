using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class SubscriptionConfigurationSpecification
    {
        [Subject(typeof(SubscriptionConfiguration))]
        public class when_building_default_consumer_strategy : WithSubject<SubscriptionConfiguration>
        {
            Because of = () =>
                strategy = Subject.BuildDeliveryStrategy(An<IDeliveryProcessor>());

            It should_build_immediate_strategy = () =>
                strategy.ShouldBeOfType<ImmediateDeliveryStrategy>();

            static IDeliveryStrategy strategy;
        }

        [Subject(typeof(SubscriptionConfiguration))]
        public class when_building_configured_consumer_strategy : WithSubject<SubscriptionConfiguration>
        {
            Establish context = () =>
                Subject.DeliverUsing<WorkerPoolDeliveryStrategy>(t => t.NumWorkers = 5);

            Because of = () =>
                strategy = Subject.BuildDeliveryStrategy(An<IDeliveryProcessor>());

            It should_create_strategy_of_correct_type = () =>
                strategy.ShouldBeOfType<WorkerPoolDeliveryStrategy>();

            It should_configured_strategy = () =>
                ((WorkerPoolDeliveryStrategy)strategy).NumWorkers.ShouldEqual(5);

            static IDeliveryStrategy strategy;
        }

        [Subject(typeof(SubscriptionConfiguration))]
        public class When_Context
        {
            Establish context = () => { };

            private Because of = () => { };

            private It should_ = () => { };
        }
    }
}
