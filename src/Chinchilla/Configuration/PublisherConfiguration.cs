using System;
using Chinchilla.Topologies;

namespace Chinchilla.Configuration
{
    public class PublisherConfiguration : EndpointConfiguration, IPublisherConfiguration, IPublisherBuilder
    {
        private Func<string, IRouter> routerBuilder = replyTo =>
            new DefaultRouter(replyTo);

        private Func<IPublishFaultStrategy> publishFaultStrategyBuilder = () =>
            new DefaultPublishFaultStrategy();

        public PublisherConfiguration()
        {
            MessageTopologyBuilder = new DefaultPublishTopologyBuilder();
            ShouldBuildTopology = true;
            ShouldConfirm = true;
        }

        public string EndpointName { get; private set; }

        public string ContentType { get; private set; }

        public string ReplyQueue { get; private set; }

        public bool ShouldBuildTopology { get; private set; }

        public bool ShouldConfirm { get; private set; }

        public IPublisherBuilder RouteWith<TRouter>()
            where TRouter : IRouter, new()
        {
            routerBuilder = replyTo => new TRouter();
            return this;
        }

        public IPublisherBuilder RouteWith(IRouter router)
        {
            routerBuilder = replyTo => router;
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

        public IPublisherBuilder ReplyTo(string queueName)
        {
            ReplyQueue = queueName;
            return this;
        }

        public IPublisherBuilder BuildTopology(bool shouldBuildTopology)
        {
            ShouldBuildTopology = shouldBuildTopology;
            return this;
        }

        public IPublisherBuilder Confirm(bool shouldConfirm)
        {
            ShouldConfirm = shouldConfirm;
            return this;
        }

        public IPublisherBuilder OnPublishFaults<TStrategy>(params Action<TStrategy>[] configurations)
            where TStrategy : IPublishFaultStrategy, new()
        {
            publishFaultStrategyBuilder = () =>
            {
                var strategy = new TStrategy();
                foreach (var builder in configurations)
                {
                    builder(strategy);
                }
                return strategy;
            };

            return this;
        }

        public IPublisherBuilder OnPublishFaults<TStrategy>(TStrategy instance)
            where TStrategy : IPublishFaultStrategy
        {
            publishFaultStrategyBuilder = () => instance;
            return this;
        }

        public IRouter BuildRouter()
        {
            return routerBuilder(ReplyQueue);
        }

        public IPublishFaultStrategy BuildFaultStrategy()
        {
            return publishFaultStrategyBuilder();
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