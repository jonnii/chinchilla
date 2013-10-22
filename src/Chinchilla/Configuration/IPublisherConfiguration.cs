namespace Chinchilla.Configuration
{
    public interface IPublisherConfiguration<TMessage> : IEndpointConfiguration
    {
        string EndpointName { get; }

        string ContentType { get; }

        string ReplyQueue { get; }

        bool ShouldBuildTopology { get; }

        bool ShouldConfirm { get; }

        IRouter BuildRouter();

        IPublisherFailureStrategy<TMessage> BuildFaultStrategy();

        IHeadersStrategy<TMessage> BuildHeaderStrategy();
    }
}