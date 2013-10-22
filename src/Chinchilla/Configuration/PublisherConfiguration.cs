using System;
using Chinchilla.Topologies;

namespace Chinchilla.Configuration
{
    public class PublisherConfiguration<TMessage> : EndpointConfiguration, IPublisherConfiguration<TMessage>, IPublisherBuilder<TMessage>
    {
        private Func<string, IRouter> routerBuilder = replyTo =>
            new DefaultRouter(replyTo);

        private Func<IPublisherFailureStrategy<TMessage>> publishFaultStrategyBuilder = () =>
            new DefaultPublisherFailureStrategy<TMessage>();

        private Func<IHeadersStrategy<TMessage>> headersStrategyBuilder = () =>
            new NullHeaderStrategy<TMessage>();

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

        public IPublisherBuilder<TMessage> OnFailure<TStrategy>(params Action<TStrategy>[] configurations)
            where TStrategy : IPublisherFailureStrategy<TMessage>, new()
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

        public IPublisherBuilder<TMessage> OnFailure<TStrategy>(IPublisherFailureStrategy<TMessage> instance)
        {
            publishFaultStrategyBuilder = () => instance;
            return this;
        }

        public IPublisherBuilder<TMessage> WithHeaders(IHeadersStrategy<TMessage> instance)
        {
            headersStrategyBuilder = () => instance;
            return this;
        }

        public IPublisherBuilder<TMessage> WithHeaders<TStrategy>(params Action<TStrategy>[] configurations)
            where TStrategy : IHeadersStrategy<TMessage>, new()
        {
            headersStrategyBuilder = () =>
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

        public IRouter BuildRouter()
        {
            return routerBuilder(ReplyQueue);
        }

        public IPublisherFailureStrategy<TMessage> BuildFaultStrategy()
        {
            return publishFaultStrategyBuilder();
        }

        public IHeadersStrategy<TMessage> BuildHeaderStrategy()
        {
            return headersStrategyBuilder();
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