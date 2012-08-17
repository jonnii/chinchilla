using Chinchilla.Topologies;
using Chinchilla.Topologies.Model;
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
                Subject.BuildTopology(new Endpoint("endpointName", "messageType")).ShouldBeOfType<DefaultTopology>();
        }

        [Subject(typeof(PublisherConfiguration))]
        public class when_building_custom_topology : WithSubject<PublisherConfiguration>
        {
            Establish context = () =>
                Subject.SetTopology(_ => new CustomTopology());

            Because of = () =>
                topology = Subject.BuildTopology(new Endpoint("endpointName", "messageType"));

            It should_build_default_topology = () =>
                topology.ShouldBeOfType<CustomTopology>();

            static IPublisherTopology topology;
        }

        public class CustomTopology : IPublisherTopology
        {
            public IExchange PublishExchange { get; set; }

            public void Visit(ITopologyVisitor visitor)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
