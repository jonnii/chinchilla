using System;
using Chinchilla.Topologies;

namespace Chinchilla.Configuration
{
    public class PublisherConfiguration<TMessage> : EndpointConfiguration, IPublisherConfiguration<TMessage>, IPublisherBuilder<TMessage>
    {
        private Func<string, IRouter> routerBuilder = replyTo =>
            new DefaultRouter(replyTo);

        private Func<IPublishFaultStrategy<TMessage>> publishFaultStrategyBuilder = () =>
            new DefaultPublishFaultStrategy<TMessage>();

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

        public IPublisherBuilder<TMessage> RouteWith<TRouter>()
            where TRouter : IRouter, new()
        {
            routerBuilder = replyTo => new TRouter();
            return this;
        }

        public IPublisherBuilder<TMessage> RouteWith(IRouter router)
        {
            routerBuilder = replyTo => router;
            return this;
        }

        public IPublisherBuilder<TMessage> PublishOn(string endpointName)
        {
            EndpointName = endpointName;
            return this;
        }

        public IPublisherBuilder<TMessage> SerializeWith(string contentType)
        {
            ContentType = contentType;
            return this;
        }

        public IPublisherBuilder<TMessage> ReplyTo(string queueName)
        {
            ReplyQueue = queueName;
            return this;
        }

        public IPublisherBuilder<TMessage> BuildTopology(bool shouldBuildTopology)
        {
            ShouldBuildTopology = shouldBuildTopology;
            return this;
        }

        public IPublisherBuilder<TMessage> Confirm(bool shouldConfirm)
        {
            ShouldConfirm = shouldConfirm;
            return this;
        }

        public IPublisherBuilder<TMessage> OnPublishFaults<TStrategy>(params Action<TStrategy>[] configurations)
            where TStrategy : IPublishFaultStrategy<TMessage>, new()
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

        public IPublisherBuilder<TMessage> OnPublishFaults<TStrategy>(TStrategy instance)
            where TStrategy : IPublishFaultStrategy<TMessage>
        {
            publishFaultStrategyBuilder = () => instance;
            return this;
        }

        public IRouter BuildRouter()
        {
            return routerBuilder(ReplyQueue);
        }

        public IPublishFaultStrategy<TMessage> BuildFaultStrategy()
        {
            return publishFaultStrategyBuilder();
        }

        public IPublisherBuilder<TMessage> SetTopology(IMessageTopologyBuilder messageTopologyBuilder)
        {
            MessageTopologyBuilder = messageTopologyBuilder;
            return this;
        }

        public override string ToString()
        {
            return string.Format("[PublisherConfiguration EndpointName={0}]", EndpointName ?? "<auto>");
        }
    }
}