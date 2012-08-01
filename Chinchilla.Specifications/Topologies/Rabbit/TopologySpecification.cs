using Chinchilla.Topologies.Rabbit;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications.Topologies.Rabbit
{
    public class TopologySpecification
    {
        [Subject(typeof(Topology))]
        public class when_defining_queue : WithSubject<Topology>
        {
            Because of = () =>
                queue = Subject.DefineQueue();

            It should_create_queue = () =>
                queue.ShouldNotBeNull();

            static IQueue queue;
        }

        [Subject(typeof(Topology))]
        public class when_defining_exchange : WithSubject<Topology>
        {
            Because of = () =>
                exchange = Subject.DefineExchange("name", ExchangeType.Fanout);

            It should_create_exchange = () =>
                exchange.ShouldNotBeNull();

            static IExchange exchange;
        }

        [Subject(typeof(Topology))]
        public class when_visiting : with_topology
        {
            Establish context = () =>
                visitor = An<ITopologyVisitor>();

            Because of = () =>
                Subject.Visit(visitor);

            It should_visit_each_queue = () =>
                visitor.WasToldTo(v => v.Visit(Param.IsAny<IQueue>())).Twice();

            It should_visit_each_exchange = () =>
                visitor.WasToldTo(v => v.Visit(Param.IsAny<IExchange>())).Twice();

            static ITopologyVisitor visitor;
        }

        public class with_topology : WithSubject<Topology>
        {
            Establish context = () =>
            {
                Subject.DefineQueue();
                Subject.DefineQueue();
                Subject.DefineExchange("exchange1", ExchangeType.Direct);
                Subject.DefineExchange("exchange2", ExchangeType.Direct);
            };
        }
    }
}
