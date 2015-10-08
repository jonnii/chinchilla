using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class ConnectionStringSpecification
    {
        [Subject(typeof(ConnectionString))]
        public class when_creating_from_uri
        {
            Because of = () =>
                connectionString = ConnectionString.Uri("amqp://username:password@hostname:portNumber/virtualHost");

            It should_create_uri_connection_string = () =>
                connectionString.ShouldBeAssignableTo<UriConnectionString>();

            static ConnectionString connectionString;
        }
    }
}
