using Chinchilla.Topologies;
using Chinchilla.Topologies.Rabbit;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class PublisherConfigurationSpecification
    {
        [Subject(typeof(PublisherConfiguration))]
        public class in_general : WithSubject<PublisherConfiguration>
        {
            It should_build_default_topology = () =>
                Subject.BuildTopology("messageType").ShouldBeOfType<DefaultPublisherTopology>();
        }

        [Subject(typeof(PublisherConfiguration))]
        public class when_building_custom_topology : WithSubject<PublisherConfiguration>
        {
            Establish context = () =>
                Subject.SetTopology(_ => new CustomTopology());

            Because of = () =>
                topology = Subject.BuildTopology("messageType");

            It should_build_default_topology = () =>
                topology.ShouldBeOfType<CustomTopology>();

            static IPublisherTopology topology;
        }

        public class CustomTopology : IPublisherTopology
        {
            public IExchange Exchange { get; set; }

            public void Visit(ITopologyVisitor visitor)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
