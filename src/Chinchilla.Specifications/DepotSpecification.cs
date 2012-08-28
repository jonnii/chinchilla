using System;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class DepotSpecification
    {
        [Subject(typeof(Depot))]
        public class when_connecting_without_amqp_uri : WithFakes
        {
            Establish context = () =>
                connectionFactory = An<IConnectionFactory>();

            Because of = () =>
                Depot.Connect(new DepotSettings
                {
                    ConnectionString = "server/host",
                    ConnectionFactoryBuilder = () => connectionFactory
                });

            It should_append_amqp_extension_if_missing = () =>
                connectionFactory.WasToldTo(f => f.Create(new Uri("amqp://server/host")));

            static IConnectionFactory connectionFactory;
        }

        [Subject(typeof(Depot))]
        public class when_connecting_with_startup_concerns : WithFakes
        {
            Establish context = () =>
            {
                concern = An<IBusConcern>();
                settings = new DepotSettings
                {
                    ConnectionString = "amqp://server/host",
                    ConnectionFactoryBuilder = () => An<IConnectionFactory>(),
                };

                settings.AddStartupConcern(concern);
            };

            Because of = () =>
                Depot.Connect(settings);

            It should_append_amqp_extension_if_missing = () =>
                concern.WasToldTo(c => c.Run(Param.IsAny<Bus>()));

            static IBusConcern concern;

            static DepotSettings settings;
        }
    }
}
