using System;
using Chinchilla.Serializers;

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
            var connectionFactory = settings.ConnectionFactoryBuilder();
            var consumerFactory = settings.ConsumerFactoryBuilder();

            var connection = connectionFactory.Create(new Uri(connectionString));
            var messageSerializer = new JsonMessageSerializer();

            return new Bus(
                connection,
                consumerFactory,
                new PublisherFactory(messageSerializer),
                new SubscriptionFactory(messageSerializer));
        }
    }
}
