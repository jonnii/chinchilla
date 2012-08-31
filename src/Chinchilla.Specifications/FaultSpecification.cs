using System;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class FaultSpecification
    {
        [Subject(typeof(Fault))]
        public class when_created
        {
            Establish context = () =>
                fault = new Fault();

            It should_have_occured_at_right_now = () =>
                fault.OccuredAt.ShouldBeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));

            static Fault fault;
        }
    }
}
