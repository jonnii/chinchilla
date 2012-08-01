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
                Depot.Connect("server/host", connectionFactory);

            It should_append_amqp_extension_if_missing = () =>
                connectionFactory.WasToldTo(f => f.Create(new Uri("amqp://server/host")));

            static IConnectionFactory connectionFactory;
        }
    }
}
