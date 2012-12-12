using System;
using Chinchilla.Topologies;

namespace Chinchilla.Configuration
{
    public class PublisherConfiguration : EndpointConfiguration, IPublisherConfiguration, IPublisherBuilder
    {
        private Func<IRouter> routerBuilder = () => new DefaultRouter();

        public PublisherConfiguration()
        {
            MessageTopologyBuilder = new DefaultPublishTopologyBuilder();
        }

        public string EndpointName { get; set; }

        public string ContentType { get; private set; }

        public IPublisherBuilder RouteWith<TRouter>()
            where TRouter : IRouter, new()
        {
            routerBuilder = () => new TRouter();
            return this;
        }

        public IPublisherBuilder RouteWith(IRouter router)
        {
            routerBuilder = () => router;
            return this;
        }

        public IPublisherBuilder PublishOn(string endpointName)
        {
            EndpointName = endpointName;
            return this;
        }

        public IPublisherBuilder SerializeWith(string contentType)
        {
            ContentType = contentType;
            return this;
        }

        public IRouter BuildRouter()
        {
            return routerBuilder();
        }

        public IPublisherBuilder SetTopology(IMessageTopologyBuilder messageTopologyBuilder)
        {
            MessageTopologyBuilder = messageTopologyBuilder;
            return this;
        }

        public override string ToString()
        {
            return string.Format("[PublishConfiguration EndpointName={0}]", EndpointName ?? "<auto>");
        }
    }
}