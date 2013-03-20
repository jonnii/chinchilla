using Chinchilla.Topologies.Model;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications.Topologies.Model
{
    public class QueueSpecification
    {
        [Subject(typeof(Queue))]
        public class in_general : with_queue
        {
            It should_not_have_bindings = () =>
                Subject.HasBindings.ShouldBeFalse();

            It should_not_have_deadletter_exchange = () =>
                Subject.DeadLetterExchange.ShouldBeNull();
        }

        [Subject(typeof(Queue))]
        public class without_name
        {
            Establish context = () =>
                queue = new Queue();

            It should_be_transient = () =>
                queue.Durability.ShouldEqual(Durability.Transient);

            It should_be_auto_delete = () =>
                queue.IsAutoDelete.ShouldBeTrue();

            It should_not_have_name = () =>
                queue.HasName.ShouldBeFalse();

            static Queue queue;
        }

        [Subject(typeof(Queue))]
        public class with_name
        {
            Establish context = () =>
                queue = new Queue("queue-name");

            It should_be_durable = () =>
                queue.Durability.ShouldEqual(Durability.Durable);

            It should_not_be_auto_delete = () =>
                queue.IsAutoDelete.ShouldBeFalse();

            static Queue queue;
        }

        [Subject(typeof(Queue))]
        public class when_binding : with_queue
        {
            Because of = () =>
                Subject.BindTo(new Exchange("exchange", ExchangeType.Fanout));

            It should_have_bindings = () =>
                Subject.HasBindings.ShouldBeTrue();
        }

        [Subject(typeof(Queue))]
        public class when_visiting : with_bound_queue
        {
            Establish context = () =>
                visitor = An<ITopologyVisitor>();

            Because of = () =>
                Subject.Visit(visitor);

            It should_visit_queue = () =>
                visitor.WasToldTo(v => v.Visit(Subject));

            It should_visit_each_binding = () =>
                visitor.WasToldTo(v => v.Visit(Param.IsAny<IBinding>()));

            static ITopologyVisitor visitor;
        }

        public class with_queue : WithFakes
        {
            Establish context = () =>
                Subject = new Queue();

            protected static Queue Subject;
        }

        public class with_bound_queue : with_queue
        {
            Establish context = () =>
                Subject.BindTo(new Exchange("foo", ExchangeType.Fanout));
        }
    }
}
