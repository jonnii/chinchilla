using Chinchilla.Topologies.Rabbit;
using Machine.Specifications;

namespace Chinchilla.Specifications.Topologies.Rabbit
{
    public class ExchangeSpecification
    {
        [Subject(typeof(Exchange))]
        public class in_general
        {
            Establish context = () =>
                exchange = new Exchange("name", ExchangeType.Fanout);

            It should_be_durable = () =>
                exchange.Durability.ShouldEqual(Durability.Durable);

            It should_not_be_auto_delete = () =>
                exchange.IsAutoDelete.ShouldBeFalse();

            It should_not_be_internal = () =>
                exchange.IsInternal.ShouldBeFalse();

            It should_not_have_alternate = () =>
                exchange.HasAlternateExchange.ShouldBeFalse();

            static Exchange exchange;
        }
    }
}
