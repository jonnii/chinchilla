namespace Chinchilla.Configuration
{
    public interface IPublisherConfiguration : IEndpointConfiguration
    {
        IRouter BuildRouter();
    }
}