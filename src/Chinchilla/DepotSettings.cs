using System;

namespace Chinchilla
{
    public class DepotSettings
    {
        private const string AmqpProtocol = "amqp://";

        public DepotSettings()
        {
            ConnectionFactoryBuilder = () => new DefaultConnectionFactory();
        }

        public string ConnectionString { get; set; }

        public Func<IConnectionFactory> ConnectionFactoryBuilder { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                throw new ChinchillaException("A connection string is required");
            }

            if (ConnectionFactoryBuilder == null)
            {
                throw new ChinchillaException("A connection factory builder is required");
            }

            if (!ConnectionString.StartsWith(AmqpProtocol))
            {
                ConnectionString = string.Concat(AmqpProtocol, ConnectionString);
            }
        }
    }
}