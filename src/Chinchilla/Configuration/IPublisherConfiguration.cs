namespace Chinchilla.Configuration
{
    public interface IPublisherConfiguration : IEndpointConfiguration
    {
        string EndpointName { get; }

        string ContentType { get; }

        string ReplyQueue { get; }

        bool ShouldBuildTopology { get; }

        bool ShouldConfirm { get; }

        IRouter BuildRouter();
    }
}