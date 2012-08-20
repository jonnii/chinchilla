namespace Chinchilla
{
    public interface IPublisherConfiguration : IEndpointConfiguration
    {
        IRouter BuildRouter();
    }
}