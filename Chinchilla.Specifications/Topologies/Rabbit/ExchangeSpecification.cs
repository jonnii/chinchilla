using Chinchilla.Topologies.Rabbit;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications.Topologies.Rabbit
{
    public class ExchangeSpecification
    {
        [Subject(typeof(Exchange))]
        public class in_general : with_exchange
        {
            It should_be_durable = () =>
                Subject.Durability.ShouldEqual(Durability.Durable);

            It should_not_be_auto_delete = () =>
                Subject.IsAutoDelete.ShouldBeFalse();

            It should_not_be_internal = () =>
                Subject.IsInternal.ShouldBeFalse();

            It should_not_have_alternate = () =>
                Subject.HasAlternateExchange.ShouldBeFalse();
        }

        [Subject(typeof(Exchange))]
        public class when_visiting : with_exchange
        {
            Establish context = () =>
                visitor = An<ITopologyVisitor>();

            Because of = () =>
                Subject.Visit(visitor);

            It should_visit_exchange = () =>
                visitor.WasToldTo(v => v.Visit(Param.IsAny<IExchange>()));

            static ITopologyVisitor visitor;
        }

        public class with_exchange : WithFakes
        {
            Establish context = () =>
                Subject = new Exchange("name", ExchangeType.Fanout);

            protected static Exchange Subject;
        }
    }
}
