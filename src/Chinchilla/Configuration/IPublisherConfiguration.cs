namespace Chinchilla.Configuration
{
    public interface IPublisherConfiguration : IEndpointConfiguration
    {
        string EndpointName { get; }

        string ContentType { get; }

        string ReplyQueue { get; }

        IRouter BuildRouter();
    }
}