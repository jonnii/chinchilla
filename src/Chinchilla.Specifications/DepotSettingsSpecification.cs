using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class DepotSettingsSpecification
    {
        [Subject(typeof(DepotSettings))]
        public class when_building_default_connection_factory
        {
            Establish context = () =>
                settings = new DepotSettings();

            Because of = () =>
                builder = settings.ConnectionFactoryBuilder();

            It should_build_default_connection_factory = () =>
                builder.ShouldBeOfType<DefaultConnectionFactory>();

            static DepotSettings settings;

            static IConnectionFactory builder;
        }

        [Subject(typeof(DepotSettings))]
        public class when_building_default_consumer_factory
        {
            Establish context = () =>
                settings = new DepotSettings();

            Because of = () =>
                builder = settings.ConsumerFactoryBuilder();

            It should_build_default_consumer_factory = () =>
                builder.ShouldBeOfType<DefaultConsumerFactory>();

            static DepotSettings settings;

            static IConsumerFactory builder;
        }
    }
}
