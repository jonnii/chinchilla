using System;

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

            return new Bus(connection);
        }
    }
}
