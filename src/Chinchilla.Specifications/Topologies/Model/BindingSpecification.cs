using Chinchilla.Topologies.Model;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications.Topologies.Model
{
    public class BindingSpecification
    {
        [Subject(typeof(Binding))]
        public class when_visiting : with_binding
        {
            Establish context = () =>
                visitor = An<ITopologyVisitor>();

            Because of = () =>
                Subject.Visit(visitor);

            It should_visit_exchange = () =>
                visitor.WasToldTo(v => v.Visit(Param.IsAny<IExchange>()));

            It should_visit_binding = () =>
                visitor.WasToldTo(v => v.Visit(Param.IsAny<IBinding>()));

            static ITopologyVisitor visitor;
        }

        public class with_binding : WithFakes
        {
            Establish context = () =>
                Subject = new Binding(new Queue("name"), new Exchange("exchange", ExchangeType.Fanout));

            protected static Binding Subject;
        }
    }
}
