using Chinchilla.Topologies;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class PublisherConfigurationSpecification
    {
        [Subject(typeof(PublisherConfiguration))]
        public class when_building_default_topology : WithSubject<PublisherConfiguration>
        {
            Because of = () =>
                messageTopology = Subject.BuildTopology(new Endpoint("endpointName", "messageType"));

            It should_build_topology = () =>
                messageTopology.ShouldNotBeNull();

            static IMessageTopology messageTopology;
        }
    }
}
