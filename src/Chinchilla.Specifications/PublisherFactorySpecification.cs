using Chinchilla.Configuration;
using Chinchilla.Topologies;
using Chinchilla.Topologies.Model;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class PublisherFactorySpecification
    {
        [Subject(typeof(PublisherFactory))]
        public class when_building_publisher : WithSubject<PublisherFactory>
        {
            Establish context = () =>
            {
                modelReference = An<IModelReference>();
                configuration = An<IPublisherConfiguration>();

                configuration.WhenToldTo(c => c.BuildRouter()).Return(An<IRouter>());
                configuration.WhenToldTo(c => c.BuildTopology(Param.IsAny<IEndpoint>()))
                    .Return(new MessageTopology{PublishExchange = An<IExchange>()});
            };

            Because of = () =>
                Subject.Create<TestMessage>(modelReference, configuration);

            It should_build_router = () =>
                configuration.WasToldTo(c => c.BuildRouter());

            static IPublisherConfiguration configuration;

            static IModelReference modelReference;
        }

        public class TestMessage { }
    }
}
