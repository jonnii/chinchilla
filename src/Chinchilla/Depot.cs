using System;
using Chinchilla.Serializers;

namespace Chinchilla
{
    public class Depot
    {
        public const string AmqpProtocol = "amqp://";

        public static IBus Connect(string connectionString)
        {
            var connectionFactory = new DefaultConnectionFactory();
            return Connect(connectionString, connectionFactory);
        }

        public static IBus Connect(string connectionString, IConnectionFactory connectionFactory)
        {
            if (!connectionString.StartsWith(AmqpProtocol))
            {
                connectionString = string.Concat(AmqpProtocol, connectionString);
            }

            var connection = connectionFactory.Create(new Uri(connectionString));
            var messageSerializer = new JsonMessageSerializer();

            return new Bus(
                connection,
                new PublisherFactory(messageSerializer),
                new SubscriptionFactory(messageSerializer));
        }
    }
}
