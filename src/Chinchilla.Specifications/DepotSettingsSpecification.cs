using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class DepotSettingsSpecification
    {
        [Subject(typeof(DepotSettings))]
        public class in_general : with_settings
        {
            It should_have_startup_concerns = () =>
                settings.StartupConcerns.ShouldNotBeNull();

            It should_have_message_serializers = () =>
                settings.MessageSerializers.ShouldNotBeNull();
        }

        [Subject(typeof(DepotSettings))]
        public class when_building_default_connection_factory : with_settings
        {
            Because of = () =>
                builder = settings.ConnectionFactoryBuilder();

            It should_build_default_connection_factory = () =>
                builder.ShouldBeAssignableTo<DefaultConnectionFactory>();

            static IConnectionFactory builder;
        }

        [Subject(typeof(DepotSettings))]
        public class when_building_default_consumer_factory : with_settings
        {
            Because of = () =>
                builder = settings.ConsumerFactoryBuilder();

            It should_build_default_consumer_factory = () =>
                builder.ShouldBeAssignableTo<DefaultConsumerFactory>();

            static IConsumerFactory builder;
        }

        [Subject(typeof(DepotSettings))]
        public class when_validating_connection_string_starting_with_rabbitmq_protocol : with_settings
        {
            Establish context = () =>
                settings.ConnectionString = "rabbitmq://foo/bar";

            Because of = () =>
                settings.Validate();

            It should_build_default_consumer_factory = () =>
                settings.ConnectionString.ShouldStartWith("amqp://");
        }

        public class with_settings
        {
            Establish context = () =>
                settings = new DepotSettings();

            protected static DepotSettings settings;
        }
    }
}
