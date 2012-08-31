using Chinchilla.Configuration;
using Chinchilla.Topologies;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications.Configuration
{
    public class SubscriptionConfigurationSpecification
    {
        [Subject(typeof(SubscriptionConfiguration))]
        public class in_general : WithSubject<SubscriptionConfiguration>
        {
            It should_have_default_prefetch_count = () =>
                Subject.PrefetchCount.ShouldEqual((ushort)50);

            It should_have_default_prefetch_size = () =>
                Subject.PrefetchSize.ShouldEqual((uint)0);
        }

        [Subject(typeof(SubscriptionConfiguration))]
        public class when_building_default_topology : WithSubject<SubscriptionConfiguration>
        {
            Because of = () =>
                messageTopology = Subject.BuildTopology(new Endpoint("endpointName", "messageType"));

            It should_build_topology = () =>
                messageTopology.ShouldNotBeNull();

            static IMessageTopology messageTopology;
        }

        [Subject(typeof(SubscriptionConfiguration))]
        public class when_building_custom_topology : WithSubject<SubscriptionConfiguration>
        {
            Establish context = () =>
            {
                builder = An<IMessageTopologyBuilder>();
                Subject.SetTopology(builder);
            };

            Because of = () =>
                Subject.BuildTopology(new Endpoint("endpointName", "messageType"));

            It should_build_default_topology = () =>
                builder.WasToldTo(b => b.Build(Param.IsAny<IEndpoint>()));

            static IMessageTopologyBuilder builder;
        }

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
        public class when_building_default_delivery_failure_strategy : WithSubject<SubscriptionConfiguration>
        {
            Because of = () =>
                strategy = Subject.BuildFaultStrategy(An<IBus>());

            It should_build_immediate_strategy = () =>
                strategy.ShouldBeOfType<ErrorQueueFaultStrategy>();

            static IFaultStrategy strategy;
        }

        [Subject(typeof(SubscriptionConfiguration))]
        public class when_building_configured_delivery_failure_strategy : WithSubject<SubscriptionConfiguration>
        {
            Establish context = () =>
                Subject.DeliverFaultsUsing<IgnoreFaultStrategy>();

            Because of = () =>
                strategy = Subject.BuildFaultStrategy(An<IBus>());

            It should_create_strategy_of_correct_type = () =>
                strategy.ShouldBeOfType<IgnoreFaultStrategy>();

            static IFaultStrategy strategy;
        }
    }
}
