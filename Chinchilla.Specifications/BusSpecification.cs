using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class BusSpecification
    {
        [Subject(typeof(Bus))]
        public class in_general : with_bus
        {
            It should_have_topology = () => 
                Subject.Topology.ShouldNotBeNull();
        }

        public class with_bus : WithSubject<Bus> { }
    }
}
