using System;

namespace Chinchilla
{
    public class Depot
    {
        public static IBus Connect(string connectionString)
        {
            var settings = new DepotSettings
            {
                ConnectionString = connectionString
            };

            return Connect(settings);
        }

        public static IBus Connect(string connectionString, DepotSettings settings)
        {
            settings.ConnectionString = connectionString;

            return Connect(settings);
        }

        public static IBus Connect(DepotSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            settings.Validate();

            var connectionString = settings.ConnectionString;
            var connectionFactory = settings.ConnectionFactoryBuilder(new[] { new Uri(connectionString) });
            var consumerFactory = settings.ConsumerFactoryBuilder();

            var modelFactory = connectionFactory.Create();
            var messageSerializers = settings.MessageSerializers;

            var bus = new Bus(
                modelFactory,
                consumerFactory,
                new RequesterFactory(),
                new PublisherFactory(messageSerializers),
                new SubscriptionFactory(modelFactory, messageSerializers));

            foreach (var concern in settings.StartupConcerns)
            {
                concern.Run(bus);
            }

            return bus;
        }
    }
}
