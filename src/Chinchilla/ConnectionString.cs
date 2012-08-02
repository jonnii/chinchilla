using RabbitMQ.Client;

namespace Chinchilla
{
    public abstract class ConnectionString
    {
        public static ConnectionString Uri(string uri)
        {
            return new UriConnectionString(uri);
        }

        public abstract void Apply(ConnectionFactory connection);
    }
}